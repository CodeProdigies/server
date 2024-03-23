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
    [Authorize(AuthenticationSchemes = "Accounts")]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly INotificationsService _notificationsService;

        public NotificationsController(IAccountService accountService, INotificationsService notificationsService)
        {
            _accountService = accountService;
            _notificationsService = notificationsService;
        }

        [HttpPost("/notifications")]
        public async Task<IResponse<bool>> Create(Notification notification)
        {
            try
            {
                await _notificationsService.Create(notification);
                return Ok<bool>("notification_created", true);
            }
            catch (Exception e)
            {
                return NotFound<bool>(e.Message, false);
            }

        }

        [HttpGet("/notifications/{id}")]
        public async Task<IResponse<Notification>> Get(Guid id)
        {
            var notification = await _notificationsService.Get(id);
            if (notification == null) return NotFound<Notification>("retrieve_notification_failed");

            return Ok<Notification>("retrieve_notification_successful", notification);
        }

        //[HttpPut("/notifications/{id}")]
        //public async Task<IResponse<bool>> MarkAsRead(Guid id)
        //{
        //    try
        //    {
        //        await _notificationsService.MarkAsRead(id);
        //        return Ok<bool>("notification_updated", true);
        //    }
        //    catch (Exception e)
        //    {
        //        return NotFound<bool>(e.Message, false);
        //    }

        //}

        [HttpDelete("/notifications/{id}")]
        [ProducesResponseType(typeof(IResponse<Product>), 400)]
        [ProducesResponseType(typeof(IResponse<Product>), 401)]
        [ProducesResponseType(typeof(IResponse<Product>), 200)]
        public async Task<IResponse<bool>> Delete(Guid id)
        {

            string? userId = this.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var account = await _accountService.GetById(userId!);
            if (account == null) return Unauthorized<bool>("failed_delete_account", false);


            var deleted = await _notificationsService.Delete(id);
            if (deleted == 0) return BadRequest<bool>("notification_deleted_notfound", false);

            return Ok<bool>("notification_deleted_successfully", true);
        }
    }
}
