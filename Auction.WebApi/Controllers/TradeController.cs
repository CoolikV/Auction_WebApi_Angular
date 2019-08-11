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
    [RoutePrefix("api/trades")]
    [Authorize]
    public class TradeController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradeService tradeService;
        readonly ITradingLotService lotService;
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public TradeController(IAdapter adapter, ITradeService tradeService, ITradingLotService lotService, IUserManager userManager,
            ICategoryService categoryService)
        {
            _adapter = adapter;
            this.tradeService = tradeService;
            this.lotService = lotService;
            this.categoryService = categoryService;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public IHttpActionResult GetTradeById(int id)
        {
            TradeDTO tradeDto;
            try
            {
                tradeDto = tradeService.GetTradeById(id);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<TradeModel>(tradeDto));
        }
        //CHANGE methods for get trades with pagination 
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult GetTrades([FromUri] PagingParameterModel pagingParameter, [FromUri] TradeFilteringModel filter)
        {
            IEnumerable<TradeDTO> tradesForPage;
            try
            {
                tradesForPage = tradeService.GetTradesForPage(pagingParameter?.PageNumber ?? 1,
                    pagingParameter?.PageSize ?? 10, filter.TradeStarts, filter.TradeEnds, filter.MaxPrice,
                    filter.LotName, out int pagesCount, out int totalItemsCount);

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
        public IHttpActionResult VerifyLotAndStartTrade(int lotId)
        {
            try
            {
                lotService.VerifyLot(lotId);
                tradeService.StartTrade(lotId);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                tradeService.RateTradingLot(rate.TradeId, User.Identity.Name, rate.Sum);
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
