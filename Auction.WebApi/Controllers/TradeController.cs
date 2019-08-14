using Auction.BusinessLogic.DTOs.Trade;
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
            try
            {
                return Ok(tradeService.GetTradeById(id));
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult GetTrades([FromUri] PagingParameterModel pagingParameter, [FromUri] TradeFilteringModel filter)
        {
            IEnumerable<TradeDTO> tradesForPage;

            tradesForPage = tradeService.GetTradesForPage(pagingParameter?.PageNumber ?? 1,
                pagingParameter?.PageSize ?? 10, filter.StartsOn, filter.EndsOn, filter.MaxPrice,
                filter.LotName, out int pagesCount, out int totalItemsCount);

            string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(pagingParameter,
            totalItemsCount, pagesCount));

            HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

            return Ok(tradesForPage);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        public IHttpActionResult StartTrade(NewTradeDTO tradeModel)
        {
            try
            {
                tradeService.StartTrade(tradeModel.LotId);
                return Ok();
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
        }

        [HttpPut]
        [Route("")]
        [Authorize]
        public IHttpActionResult Rate([FromBody] RateDTO rate)
        {
            try
            {
                tradeService.RateTradingLot(rate, User.Identity.Name);
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