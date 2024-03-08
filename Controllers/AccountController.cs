using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            
            if(account == null) return Unauthorized<string>("Invalid Username or Password");

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
            if(!registerModel.isPasswordValid()) return BadRequest<string>("Password is not valid");
            if(!registerModel.IsValidEmail()) return BadRequest<string>("Email address is invalid");

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

        [HttpGet("/account/myuser")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<>), 404)]
        [ProducesResponseType(typeof(IResponse<Account>), 200)]
        public async Task<IResponse<Account>> GetMyUser()
        {
            string? userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized<Account>("User not found");

            var account = await _accountService.GetRequestor(userId);
            if (account == null) return NotFound<Account>("Account not found");

            // Clear account
            return Ok<Account>("account_found", account);
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

            if(token == null) return BadRequest<string>("Invalid OTP");
            if(token.ExpiresAt < DateTime.UtcNow) return BadRequest<string>("OTP has expired");

            switch(token.Type)
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
    }
}
