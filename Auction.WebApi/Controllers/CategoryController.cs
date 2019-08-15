using Auction.BusinessLogic.DTOs.Category;
using Auction.BusinessLogic.DTOs.TradingLot;
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
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradingLotService lotService;
        readonly ICategoryService categoryService;

        public CategoryController(IAdapter adapter, ITradingLotService lotService, ICategoryService categoryService)
        {
            _adapter = adapter;
            this.lotService = lotService;
            this.categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}")]
        public IHttpActionResult GetCategoryById(int id)
        {
            try
            {
                return Ok(categoryService.GetCategoryById(id));
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
        public IHttpActionResult GetCategories()
        {
            try
            {
                return Ok(categoryService.GetCategories());
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
        [Route("{id:int}/lots")]
        public IHttpActionResult GetLotsForCategory(int id, [FromUri] PagingParameterModel pagingParameter, [FromUri] LotFilteringModel filterModel)
        {
            IEnumerable<TradingLotDTO> lotsForPage;

            lotsForPage = lotService.GetLotsForPage(pagingParameter?.PageNumber ?? 1, pagingParameter?.PageSize ?? 10,
                id, filterModel.MinPrice, filterModel.MaxPrice, filterModel.LotName,
                out int pagesCount, out int totalItemsCount);

            string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(pagingParameter,
            totalItemsCount, pagesCount));

            HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);
            
            return Ok(lotsForPage);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id:int}/lots/{lotId:int}")]
        public IHttpActionResult GetLotForCategory(int id, int lotId)
        {
            TradingLotDTO lotDto;
            try
            {
                lotDto = categoryService.GetLotFromCategory(id, lotId);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(lotDto);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles ="manager,admin")]
        public IHttpActionResult AddNewCategory(NewCategoryDTO category)
        {
            try
            {
                categoryService.CreateCategory(category);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
            }
            catch (AuctionException ex)
            {
                return BadRequest(ex.Message);
            }

            return StatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles ="manager,admin")]
        public IHttpActionResult UpdateCategoryName(int id, NewCategoryDTO category)
        {
            try
            {
                categoryService.ChangeCategoryName(id, category.Name);
            }
            catch (DatabaseException)
            {
                return StatusCode(HttpStatusCode.InternalServerError);
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
 
        [HttpDelete]
        [Authorize(Roles ="manager,admin")]
        [Route("{id:int}")]
        public IHttpActionResult DeleteCategory(int id)
        {
            try
            {
                categoryService.RemoveCategoryById(id);
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

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}