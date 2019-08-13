using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
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
            get
            {
                try
                {
                    return userManager.GetUserProfileByUserName(User.Identity.Name);
                }
                catch (NotFoundException)
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetProfileInfo()
        {
            try
            {
                return Ok(_adapter.Adapt<UserProfileModel>(CurrentUser));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("lots")]
        public IHttpActionResult GetUserLots([FromUri]PagingParameterModel paging, [FromUri] LotFilteringModel filter)
        {
            IEnumerable<TradingLotDTO> lotsForPage;
            try
            {
                lotsForPage = lotService.GetLotsForUser(CurrentUser.Id, paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                    filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.LotName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<IEnumerable<TradingLotModel>>(lotsForPage));
        }

        [HttpGet]
        [Route("trades")]
        public IHttpActionResult GetTrades([FromUri] PagingParameterModel paging, [FromUri] TradeFilteringModel filter, string state)
        {
            IEnumerable<TradeDTO> tradesForPage;
            try
            {
                tradesForPage = tradeService.GetUserTrades(CurrentUser.Id, paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                    state, filter.TradeStarts, filter.TradeEnds, filter.MaxPrice, filter.LotName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<IEnumerable<TradeModel>>(tradesForPage));
        }

        //add put update delete methods...
    }
}