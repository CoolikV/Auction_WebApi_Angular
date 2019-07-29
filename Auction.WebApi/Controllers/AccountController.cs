using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        readonly IUserManager UserManager;
        readonly IAdapter Adapter;

        IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController(IUserManager userManager, IAdapter adapter)
        {
            UserManager = userManager;
            Adapter = adapter;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var user = new UserDTO() { Email = "test@mail", Password = "qwerty123", Role = "user", UserName = "coolik" };

            var result = await UserManager.CreateUserAsync(user);

            if (!result.Succedeed)
                return BadRequest(result.Message);

            return Ok();
        }

        [AllowAnonymous]
        [Route("login", Name = "login")]
        public async Task<IHttpActionResult> Login([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid data");
            }

            var claim = await UserManager.Authenticate(model.UserName, model.Password);

            Authentication.SignIn(claim);

            return Ok();
        }
    }
}
