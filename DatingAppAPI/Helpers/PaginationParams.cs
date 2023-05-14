namespace DatingAppAPI.Helpers
{
    public class PaginationParams
    {
        private const int MAX_PAGE_SIZE = 50;
        private int _pageNumber = 1;
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 0 ? _pageNumber : value; }
        }

        private int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value < MAX_PAGE_SIZE ? (value > 0 ? value : _pageSize) : MAX_PAGE_SIZE; }
        }
    }
}