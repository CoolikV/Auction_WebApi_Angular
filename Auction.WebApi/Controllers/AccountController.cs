using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        readonly IUserManager userManager;
        readonly IAdapter _adapter;


        public AccountController(IUserManager userManager, IAdapter adapter)
        {
            this.userManager = userManager;
            _adapter = adapter;
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

            var newUserDto = _adapter.Adapt<UserDTO>(registerModel);

            var result = await userManager.CreateUserAsync(newUserDto);

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
    }
}
