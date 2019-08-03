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
    [RoutePrefix("api/categories")]
    public class CategoryController : ApiController
    {
        readonly IAdapter _adapter;

        readonly ITradingLotService lotService;
        readonly IUserManager userManager;
        readonly ICategoryService categoryService;

        public CategoryController(IAdapter adapter, ITradingLotService lotService, IUserManager userManager, ICategoryService categoryService)
        {
            _adapter = adapter;
            this.lotService = lotService;
            this.userManager = userManager;
            this.categoryService = categoryService;
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult AddNewCategory(TradingLotModel newTradingLot)
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
    }
}
