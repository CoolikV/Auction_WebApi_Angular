using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using System.Threading.Tasks;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        readonly IUserManager UserManager;
        readonly IAdapter Adapter;


        public AccountController(IUserManager userManager, IAdapter adapter)
        {
            UserManager = userManager;
            Adapter = adapter;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var newUserDto = Adapter.Adapt<UserDTO>(registerModel);

            var result = await UserManager.CreateUserAsync(newUserDto);

            if (!result.Succedeed)
                return BadRequest(result.Message);

            return Ok();
        }
    }
}
