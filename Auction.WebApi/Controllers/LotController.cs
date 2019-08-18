using Auction.BusinessLogic.DTOs.TradingLot;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Helpers;
using Auction.WebApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/lots")]
    public class LotController : ApiController
    {
        readonly ITradingLotService lotService;
        readonly ICategoryService categoryService;

        public LotController(ITradingLotService lotService, ICategoryService categoryService)
        {
            this.lotService = lotService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}")]
        public IHttpActionResult GetTradingLot(int id)
        {
            try
            {
                return Ok(lotService.GetLotById(id));
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
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult GetTradingLots([FromUri] PagingParameterModel paging, [FromUri] LotFilteringModel filterModel)
        {
            IEnumerable<TradingLotDTO> lotsForPage;

            lotsForPage = lotService.GetLotsForPage(paging?.PageNumber ?? 1, paging?.PageSize ?? 10, filterModel.CategoryId,
                filterModel.MinPrice, filterModel.MaxPrice, filterModel.LotName, out int pagesCount, out int totalItemsCount);

            string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging,
                totalItemsCount, pagesCount));

            HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

            return Ok(lotsForPage);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{lotId:int}/category")]
        public IHttpActionResult GetCategoryByLotId(int lotId)
        {
            try
            {
                return Ok(lotService.GetLotById(lotId).Category);
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

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddNewTradingLot([FromBody]NewTradingLotDTO newTradingLot)
        {
            try
            {
                //REFACTORING and add pictures saving to app_data/static/pictures
                lotService.CreateLot(newTradingLot, User.Identity.Name);
                return Created(nameof(GetTradingLot), newTradingLot);
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
        [Route("{id:int}")]
        public IHttpActionResult UpdateTradingLot(int id, [FromBody]NewTradingLotDTO newTradingLot)
        {
            try
            {
                lotService.EditLot(id, newTradingLot, User.IsInRole("manager"));
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

        [HttpPatch]
        [Route("{id:int}")]
        [Authorize(Roles ="manager,admin")]
        public IHttpActionResult VerifyLot(int id)
        {
            try
            {
                lotService.VerifyLot(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteTradingLot(int id)
        {
            try
            {
                lotService.RemoveLotById(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
        }
    }
}