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
using Auction.WebApi.Helpers;

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
                //REFACTORING
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
        public IEnumerable<TradingLotModel> GetTradingLots([FromUri] PagingParameterModel pagingParameter)
        {
            //maybe remove such constructions (pagingParameter?.PageNumber ?? 1 and pagingParameter?.PageSize ?? 10) inside method
            var lotsForPage = lotService.GetLotsForPage(pagingParameter?.PageNumber ?? 1,
                pagingParameter?.PageSize ?? 10, null, out int pagesCount, out int totalItemsCount);

            string metadata = JsonConvert.SerializeObject(PaginationHelper.GeneratePageMetadata(pagingParameter, 
                totalItemsCount,pagesCount));

            HttpContext.Current.Response.Headers.Add("Paging-Headers", metadata);

            return _adapter.Adapt<IEnumerable<TradingLotModel>>(lotsForPage);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{lotId:int}/category")]
        public IHttpActionResult GetCategoryByLotId(int lotId)
        {
            CategoryModel category;
            try
            {
                var categoryDto = lotService.GetLotById(lotId).Category;
                category = _adapter.Adapt<CategoryModel>(categoryDto);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok(category);
        } 
    }
}
