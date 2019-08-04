using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
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
        public IHttpActionResult GetTrades()
        {

        }

        [HttpPut]
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
    }
}
