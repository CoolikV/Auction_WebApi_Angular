using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [RoutePrefix("api/trades")]
    public class TradeController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradeService tradeService;
        readonly ITradingLotService lotService;
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public TradeController(IAdapter adapter, ITradeService tradeService, ITradingLotService lotService, IUserManager userManager, ICategoryService categoryService)
        {
            _adapter = adapter;
            this.tradeService = tradeService;
            this.lotService = lotService;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize()]
        public IHttpActionResult GetTradeById(int id)
        {
            TradeDTO tradeDto; 
            try
            {
                tradeDto = tradeService.GetTradeById(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<TradeModel>(tradeDto));
        }

        [HttpGet]
        [Route("")]
        [Authorize()]
        public IHttpActionResult GetTrades([FromUri] PagingParameterModel pagingParameter, int? category)
        {
            IEnumerable<TradeDTO> tradesForPage;
            try
            {
                tradesForPage = tradeService.GetTradesForPage(pagingParameter?.PageNumber ?? 1,
                    pagingParameter?.PageSize ?? 10, category, out int pagesCount, out int totalItemsCount);

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

        [HttpPost]
        [Route("{lotId:int}")]
        [Authorize()]
        public IHttpActionResult StartTrade(int lotId)
        {
            try
            {
                tradeService.StartTrade(lotId);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (AuctionException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPut]
        [Route("")]
        [Authorize]
        public IHttpActionResult Rate([FromBody] RateModel rate)
        {
            try
            {
                var userId = userManager.GetUserByName(User.Identity.Name).Id;
                tradeService.RateTradingLot(rate.TradeId, userId, rate.Sum);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch(AuctionException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
