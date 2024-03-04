using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;
using prod_server.Services.DB;

namespace prod_server.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("/account/login")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<>), 200)]
        public async Task<IResponse<string>> Login(LoginModel loginModel)
        {

            var account = await _accountService.GetAccountByUsername(loginModel.Username);

            if (account == null)
                account = await _accountService.GetAccountByEmail(loginModel.Username);
            
            if(account == null) return Unauthorized<string>("Invalid Username or Password");

            if (!account.ValidatePassword(loginModel.Password))
                return Unauthorized<string>("Invalid Username or Password");

            return Ok<string>("login_success", account.CreateJwtToken());
        }

        [HttpPost("/register")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<Account>), 200)]
        public async Task<IResponse<string>> Register(RegisterModel registerModel)
        {
            //Should return auth token.
            if(!registerModel.isPasswordValid()) return BadRequest<string>("Password is not valid");
            if(!registerModel.IsValidEmail()) return BadRequest<string>("Email address is invalid");

            if (await _accountService.GetAccountByUsername(registerModel.Username) != null)
                return BadRequest<string>("Username already exists");

            // Check if email already exists
            if (await _accountService.GetAccountByEmail(registerModel.Email) != null)
                return BadRequest<string>("Email already exists");

            var createdAccount = await _accountService.Create(registerModel);
            if (createdAccount == null) return UnexpectedError<string>("Failed to create account due to server error.");

            return Ok<string>("register_success", createdAccount.CreateJwtToken());
        }
    }
}
