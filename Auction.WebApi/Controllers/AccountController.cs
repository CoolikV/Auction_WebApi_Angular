using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        readonly IUserManager userManager;
        readonly IAdapter _adapter;


        public AccountController(IUserManager userManager, IAdapter adapter)
        {
            this.userManager = userManager;
            _adapter = adapter;
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        [Route("")]
        public IHttpActionResult GetUsersList([FromUri] PagingParameterModel paging, [FromUri] string userName = "")
        {
            IEnumerable<UserDTO> usersForPage;

            try
            {
                usersForPage = userManager.GetUsersForPage(paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                    userName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
            return Ok(_adapter.Adapt<IEnumerable<UserProfileModel>>(usersForPage));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            try
            {
            var result = await userManager.CreateUserAsync(_adapter.Adapt<UserDTO>(registerModel));

            if (!result.Succedeed)
                return BadRequest(result.Message);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            return Ok();
        }
        
        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{userName}")]
        public IHttpActionResult DeleteUserAccount(string userName)
        {
            try
            {
                userManager.DeleteUserAccount(userName);
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
