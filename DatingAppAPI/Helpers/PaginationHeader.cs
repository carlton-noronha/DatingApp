namespace DatingAppAPI.Helpers
{
    public class PaginationHeader
    {
        public PaginationHeader(int currentPage, int itemsPerPage, long totalItems, int totalPage)
        {
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalItems = totalItems;
            TotalPages = totalPage;
        }

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public long TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}