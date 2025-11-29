namespace Authentication.RefreshToken.Concerns.Common
{
    public class PagedResponse<T> : ApiResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; } // TotalRecords es válido, TotalRecords es más común
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;

        public PagedResponse(T data, string message, int pageNumber, int pageSize, int totalRecords)
        {
            this.Data = data;
            this.Message = message;
            this.Succeeded = true;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
            this.TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
        }
    }
}
