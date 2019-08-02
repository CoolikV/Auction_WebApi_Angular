using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/lots")]
    public class LotsController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradingLotService lotService;
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public LotsController(IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ICategoryService categoryService)
        {
            _adapter = adapter;
            this.lotService = lotService;
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddNewTradingLot(TradingLotModel newTradingLot)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data");

            try
            {
                //refactoring
                var currentUserName = Request.GetOwinContext().Request.User.Identity.Name;
                var currentUser = userManager.GetUserByName(currentUserName);

                var lotDto = _adapter.Adapt<TradingLotDTO>(newTradingLot);

                lotDto.Category = categoryService.GetCategoryById(newTradingLot.CategoryId);

                lotDto.User = currentUser;

                lotService.CreateLot(lotDto);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
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
                return BadRequest("Invalid data");

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

            return StatusCode(HttpStatusCode.NoContent);
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
            catch(NotFoundException)
            {
                return NotFound();
            }

            return Ok(lot);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("")]
        public IEnumerable<TradingLotModel> GetTradingLots([FromUri] PagingParameterModel pagingParameter, string category)
        {
            int? currentPage = pagingParameter?.PageNumber ?? 1;
            int? pageSize = pagingParameter?.PageSize ?? 10;

            var lotsForPage = lotService.GetLotsForPage(currentPage.Value, pageSize.Value, category,
                out int pagesCount, out int totalItemsCount);

            bool hasPreviousPage = currentPage > 1 ? true : false;
            bool hasNextPage = currentPage < pagesCount ? true : false;

            var paginationMetadata = new
            {
                totalItemsCount,
                pageSize,
                pagesCount,
                hasPreviousPage,
                hasNextPage
            };

            HttpContext.Current.Response.Headers.Add("Paging-Headers",
                JsonConvert.SerializeObject(paginationMetadata));

            return _adapter.Adapt<IEnumerable<TradingLotModel>>(lotsForPage);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{lotId:int}/category")]
        public IHttpActionResult GetCategoryByLotId(int lotId)
        {
            CategoryDTO category;
            try
            {
                category = lotService.GetLotById(lotId).Category;
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(category);
        } 
    }
}
