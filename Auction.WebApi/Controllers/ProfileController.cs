using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/profile")]
    [Authorize]
    public class ProfileController : ApiController
    {
        readonly IUserManager userManager;
        readonly ITradeService tradeService;
        readonly ITradingLotService lotService;
        readonly IAdapter _adapter;

        public ProfileController(IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ITradeService tradeService)
        {
            _adapter = adapter;
            this.lotService = lotService;
            this.userManager = userManager;
            this.tradeService = tradeService;
        }

        UserDTO CurrentUser
        {
            get => userManager.GetUserProfileByEmail(User.Identity.Name);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProfileInfo()
        {
            var user = CurrentUser;
            return Ok(_adapter.Adapt<UserProfileModel>(CurrentUser));
        }

        [HttpGet]
        [Route("lots")]
        public IEnumerable<TradingLotModel> GetUserLots()
        {
            return _adapter.Adapt<IEnumerable<TradingLotModel>>(CurrentUser.TradingLots);
        }

        //add filtering model and use it to display all/win/loose/active trades
        [HttpGet]
        [Route("trades")]
        public IHttpActionResult GetTrades(PagingParameterModel pagingParameter, string state)
        {
            return null;
        }
    }
}