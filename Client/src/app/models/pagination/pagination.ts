export interface Pagination {
    CurrentPage: number;
    TotalItemsCount: number;
    PagesCount: number;
    PageSize: number;
    HasNextPage: boolean;
    HasPreviousPage: boolean;
}
