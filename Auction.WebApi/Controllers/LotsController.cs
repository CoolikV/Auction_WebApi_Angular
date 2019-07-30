using Auction.BusinessLogic.DataTransfer;
using Auction.BusinessLogic.Exceptions;
using Auction.BusinessLogic.Interfaces;
using Auction.WebApi.Models;
using Mapster;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Auction.WebApi.Controllers
{
    //[Authorize]
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
                var currentUser = userManager.GetUserByName(User.Identity.Name);
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
        public IEnumerable<TradingLotModel> GetTradingLots([FromUri] PagingParameterModel pagingParameter, int? categoryId)
        {
            //var source = lotService.FindLots();
            var source = categoryId.HasValue ? lotService.FindLots(categoryId.Value)
                : lotService.FindLots(null);

            int totalCount = source.Count();
            int currentPage = pagingParameter.PageNumber;
            int pageSize = pagingParameter.PageSize;

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var lotsForPage = source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            bool hasPreviousPage = currentPage > 1 ? true : false;
            bool hasNextPage = currentPage < totalPages ? true : false;

            var paginationMetadata = new
            {
                totalCount,
                pageSize,
                totalPages,
                hasPreviousPage,
                hasNextPage
            };

            HttpContext.Current.Response.Headers.Add("Paging-Headers",
                JsonConvert.SerializeObject(paginationMetadata));

            return _adapter.Adapt<IEnumerable<TradingLotModel>>(lotsForPage);
        }
    }
}
