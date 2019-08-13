using Auction.WebApi.Models;

namespace Auction.WebApi.Helpers
{
    public static class PaginationHelper
    {
        public static object GeneratePageMetadata(PagingParameterModel parameter, int itemsCount, int pagesCount)
        {
            int currentPage = parameter.PageNumber;

            var metadata = new
            {
                CurrentPage = currentPage,
                TotalItemsCount = itemsCount,
                PagesCount = pagesCount,
                parameter.PageSize,
                HasNextPage = currentPage < pagesCount ? true : false,
                HasPreviousPage = currentPage > 1 ? true : false
            };

            return metadata;
        }
    }
}