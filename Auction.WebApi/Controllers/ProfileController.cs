using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
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
            return Ok(_adapter.Adapt<UserProfileModel>(CurrentUser));
        }

        [HttpGet]
        [Route("lots")]
        public IEnumerable<TradingLotModel> GetUserLots(PagingParameterModel pagingParameter)
        {
            return _adapter.Adapt<IEnumerable<TradingLotModel>>(CurrentUser.TradingLots);
        }

        //add filtering model and use it to display all/win/loose/active trades
        [HttpGet]
        [Route("trades")]
        public IHttpActionResult GetTrades(PagingParameterModel pagingParameter, string state)
        {
            IEnumerable<TradeDTO> tradesForPage;
            try
            {
                tradesForPage = tradeService.GetTradesForPage(CurrentUser.Id, pagingParameter?.PageNumber ?? 1,
                    pagingParameter?.PageSize ?? 10, state, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(pagingParameter,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<IEnumerable<TradeModel>>(tradesForPage));
        }
    }
}