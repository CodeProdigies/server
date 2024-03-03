using Microsoft.AspNetCore.Mvc;
using prod_server.Classes;
using prod_server.Classes.Others;
using prod_server.Entities;

namespace prod_server.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : BaseController
    {
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginModel"></param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<>), 200)] 
        public IActionResult Login(LoginModel loginModel)
        {
            return Unauthorized();       
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(IResponse<>), 401)]
        [ProducesResponseType(typeof(IResponse<Account>), 200)]
        public IActionResult Register(RegisterModel registerModel)
        {
            //Should return auth token.
            return Ok(new Account(registerModel));
        }

    }
}
