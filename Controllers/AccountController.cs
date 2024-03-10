using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Security.Claims;

namespace prod_server.Controllers
{
    [Authorize(AuthenticationSchemes = "Accounts")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _accountService = accountService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("/account/login")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<>), 200)]
        public async Task<IResponse<string>> Login(LoginModel loginModel)
        {

            var account = await _accountService.GetByUsername(loginModel.Username);

            var deviceInfo = this.Request.Headers["User-Agent"].ToString();

            if (account == null)
                account = await _accountService.GetByEmail(loginModel.Username);

            if (account == null) return Unauthorized<string>("Invalid Username or Password");

            if (!account.ValidatePassword(loginModel.Password))
                return Unauthorized<string>("Invalid Username or Password");

            return Ok<string>("login_success", account.CreateJwtToken());
        }

        [AllowAnonymous]
        [HttpPost("/account/signup")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<Account>), 200)]
        public async Task<IResponse<string>> Register(RegisterModel registerModel)
        {
            //Should return auth token.
            if (!registerModel.isPasswordValid()) return BadRequest<string>("Password is not valid");
            if (!registerModel.IsValidEmail()) return BadRequest<string>("Email address is invalid");

            if (await _accountService.GetByUsername(registerModel.Username) != null)
                return BadRequest<string>("Username already exists");

            // Check if email already exists
            if (await _accountService.GetByEmail(registerModel.Email) != null)
                return BadRequest<string>("Email already exists");

            var deviceInfo = this.Request.Headers["User-Agent"].ToString();

            var createdAccount = await _accountService.Create(registerModel);
            if (createdAccount == null) return UnexpectedError<string>("Failed to create account due to server error.");

            return Ok<string>("register_success", createdAccount.CreateJwtToken());
        }

        /// <summary>
        /// If no id is provided, the id will be grabbed from the token and return the requestor user data.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/account/{id?}")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<>), 404)]
        [ProducesResponseType(typeof(IResponse<Account>), 200)]
        public async Task<IResponse<Account>> GetMyUser(int? id)
        {
            if(id == null)
            {
                // If no id is provided, get the id from the token.
                // It'll return it's own user data.
                string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized<Account>("retrieve_account_failed");
                id = int.Parse(userId);
            }

            var account = await _accountService.GetById(id.Value);
            if (account == null) return NotFound<Account>("retrieve_account_failed");

            // Clear account
            return Ok<Account>("retrieve_account_successful", account);
        }

        [AllowAnonymous]
        [HttpGet("/account/forgotpassword")]
        [ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IResponse<string>> ForgotPassword(string email)
        {
            var user = await _accountService.GetByEmail(email);

            // This part of the code needs runs on a different thread
            // This is to prevent the client from waiting for the email to be sent
            // OR knowing if user exists or not because of the time it takes to send an email.
            if (user != null)
            {
                Console.WriteLine("AAA");
                var token = await _tokenService.Create(user);
                // Start the asynchronous operation on a separate thread without blocking the current thread
            }

            // Return a quick response to the client
            return Ok<string>("email_sent", "account");
        }


        [AllowAnonymous]
        [HttpGet("/account/verifyotp")]
        [ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IResponse<string>> ForgotPassword(string otp, string email)
        {
            var token = await _tokenService.GetByIdAndEmail(Guid.Parse(otp), email);

            if (token == null) return BadRequest<string>("Invalid OTP");
            if (token.ExpiresAt < DateTime.UtcNow) return BadRequest<string>("OTP has expired");

            switch (token.Type)
            {
                case OTPType.ForgotPassword:
                    // Reset password?
                    break;
                case OTPType.Login:
                    // allow login?
                    break;
            }

            return Ok<string>("otp_verified", "account");
        }

        [HttpPut("/account/changepassword")]
        [ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IResponse<bool>> ChangePassword(ChangePasswordRequestModel request )
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized<bool>("failed_update_password", false);

            var user = await _accountService.GetById(userId);
            if (user == null) return BadRequest<bool>("failed_update_password", false);

            var isPreviousPasswordCorrect = user.ValidatePassword(request.OldPassword);
            if(!isPreviousPasswordCorrect) return BadRequest<bool>("failed_update_password_incorrect_previous", false);

            user.Password = Utilities.HashPassword(request.NewPassword);
            await _accountService.Update(user);

            return Ok<bool>("password_updated_successfully", true);
        }

        [HttpPut("/account")]
        [ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IResponse<bool>> Update(Account account)
        {

            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized<bool>("failed_update_account", false);
            if(userId != account.Id.ToString()) return Unauthorized<bool>("failed_update_account", false);   // Or check if is admin.

            var dbAccount = await _accountService.GetById(userId);
            if (dbAccount == null) return BadRequest<bool>("failed_update_password", false);

            dbAccount.UpdateFromAnotherAccount(account);

            await _accountService.Update(dbAccount);

            return Ok<bool>("account_updated_successfully", true);
        }

        [HttpDelete("/account/{id}")]
        [ProducesResponseType(typeof(IResponse<string>), 200)]
        public async Task<IResponse<bool>> Update(Guid id)
        {

            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized<bool>("failed_update_account", false);
            // Check if admin.

            var deleted = await _accountService.Delete(id);

            if (deleted == 0) return BadRequest<bool>("failed_delete_account_notfound", false);

            return Ok<bool>("account_updated_successfully", true);
        }
    }
}
