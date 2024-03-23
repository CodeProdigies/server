using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using prod_server.Classes;
using prod_server.Entities;
using prod_server.Services.DB;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using static prod_server.Classes.BaseController;

namespace prod_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IQuoteService _quoteService;
        private readonly INotificationsService _notificationsService;

        public GeneralController(IQuoteService quoteService, INotificationsService notificationsService)
        {
            _quoteService = quoteService;
            _notificationsService = notificationsService;
        }
    }
}
