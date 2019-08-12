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
        public IHttpActionResult GetUsersList([FromUri] PagingParameterModel paging, [FromUri] string name)
        {
            IEnumerable<UserDTO> usersForPage;

            try
            {
                usersForPage = userManager.GetUsersForPage(paging?.PageNumber ?? 1, paging?.PageSize ?? 10, name, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException ex)
            {
                HttpContext.Current.Response.Headers.Add("Error message", ex.Message);
                return NotFound();
            }
            return Ok(_adapter.Adapt<UserProfileModel>(usersForPage));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await userManager.CreateUserAsync(_adapter.Adapt<UserDTO>(registerModel));

            if (!result.Succedeed)
                return BadRequest(result.Message);

            return Ok();
        }

        [HttpPatch]
        [Authorize]
        [Route("{id}")]
        public IHttpActionResult UpdateUserProfile(string id, UserProfileModel userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var adapt = _adapter.Adapt<UserDTO>(userModel);

                userManager.EditUserProfile(id, adapt);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public IHttpActionResult DeleteUserAccount(string id)
        {
            try
            {
                userManager.DeleteUserAccount(id);
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
