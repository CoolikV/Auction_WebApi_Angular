using Auction.BusinessLogic.DTOs.Trade;
using Auction.BusinessLogic.DTOs.TradingLot;
using Auction.BusinessLogic.DTOs.UserProfile;
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
    [RoutePrefix("api/profiles")]
    [Authorize]
    public class ProfileController : ApiController
    {
        readonly IUserManager userManager;
        readonly ITradeService tradeService;
        readonly ITradingLotService lotService;

        public ProfileController(IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ITradeService tradeService)
        {
            this.lotService = lotService;
            this.userManager = userManager;
            this.tradeService = tradeService;
        }

        
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult GetProfileInfo(string id)
        {
            try
            {
                return Ok(userManager.GetUserProfileById(id));
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("{id}/lots")]
        public IHttpActionResult GetUserLots(string id, [FromUri]PagingParameterModel paging, [FromUri] LotFilteringModel filter)
        {
            IEnumerable<TradingLotDTO> lotsForPage;
            try
            {
                lotsForPage = lotService.GetLotsForUser(id, paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                    filter.CategoryId, filter.MinPrice, filter.MaxPrice, filter.LotName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

                return Ok(lotsForPage);
            }
            catch (AuctionException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id}/trades")]
        public IHttpActionResult GetTrades(string id, [FromUri] PagingParameterModel paging, [FromUri] TradeFilteringModel filter, string state = "all")
        {
            IEnumerable<TradeDTO> tradesForPage;
            try
            {
                tradesForPage = tradeService.GetUserTrades(id, paging?.PageNumber ?? 1, paging?.PageSize ?? 10,
                    state, filter.StartsOn, filter.EndsOn, filter.MaxPrice, filter.LotName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

                return Ok(tradesForPage);
            }
            catch (AuctionException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("id")]
        public IHttpActionResult UpdateUserProfile(string id, NewUserProfileDTO profileDto)
        {
            try
            {
                userManager.EditUserProfile(id, profileDto);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }
    }
}