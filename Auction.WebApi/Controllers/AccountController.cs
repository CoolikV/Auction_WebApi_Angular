using Auction.BusinessLogic.DTOs.UserProfile;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        readonly IUserManager userManager;


        public AccountController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        [Route("")]
        public IHttpActionResult GetUsersList([FromUri] PagingParameterModel paging, [FromUri] string userName = "")
        {
            IEnumerable<UserDTO> usersForPage;

            usersForPage = userManager.GetUsersForPage(paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                userName, out int pagesCount, out int totalItemsCount);

            string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
            totalItemsCount, pagesCount));

            HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

            return Ok(usersForPage);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public IHttpActionResult DeleteUserAccount(string userId)
        {
            try
            {
                userManager.DeleteUserAccount(userId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //add put update delete methods...

    }
}
