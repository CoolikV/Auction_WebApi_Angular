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
    [Authorize]
    [RoutePrefix("api/lots")]
    public class LotController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradingLotService lotService;
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public LotController(IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ICategoryService categoryService)
        {
            _adapter = adapter;
            this.lotService = lotService;
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}")]
        public IHttpActionResult GetTradingLot(int id)
        {
            TradingLotModel lot;
            try
            {
                lot = _adapter.Adapt<TradingLotModel>(lotService.GetLotById(id));
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch(NotFoundException)
            {
                return NotFound();
            }

            return Ok(lot);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public IHttpActionResult GetTradingLots([FromUri] PagingParameterModel paging, [FromUri] LotFilteringModel filterModel)
        {
            IEnumerable<TradingLotDTO> lotsForPage;
            try
            {
                lotsForPage = lotService.GetLotsForPage(paging?.PageNumber ?? 1, paging?.PageSize ?? 10, filterModel.CategoryId, 
                    filterModel.MinPrice, filterModel.MaxPrice, filterModel.LotName, out int pagesCount, out int totalItemsCount);

                string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(paging, 
                    totalItemsCount,pagesCount));

                HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(_adapter.Adapt<IEnumerable<TradingLotModel>>(lotsForPage));
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{lotId:int}/category")]
        public IHttpActionResult GetCategoryByLotId(int lotId)
        {
            CategoryModel category;
            try
            {
                category = _adapter.Adapt<CategoryModel>(lotService.GetLotById(lotId).Category);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(category);
        } 

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddNewTradingLot(BaseTradingLotModel lotModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //REFACTORING and add pictures saving to app_data/static/pictures
                var lotDto = _adapter.Adapt<TradingLotDTO>(lotModel);
                lotDto.Category = categoryService.GetCategoryById(lotModel.CategoryId);
                lotDto.User = userManager.GetUserByUserName(User.Identity.Name);
                lotService.CreateLot(lotDto);
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

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateTradingLot(int id, [FromBody]TradingLotModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                lotService.EditLot(id, _adapter.Adapt<TradingLotDTO>(model));
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

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteTradingLot(int id)
        {
            try
            {
                lotService.RemoveLotById(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}