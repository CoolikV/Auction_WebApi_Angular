using Auction.BusinessLogic.DTOs.Authorization;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/register")]
    public class RegisterController : ApiController
    {
        readonly IUserManager userManager;

        public RegisterController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Register(UserRegisterDTO user)
        {
            try
            {
                var result = await userManager.CreateUserAsync(user);

                if (!result.Succedeed)
                    return BadRequest(result.Message);

                return Ok();
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}
