namespace DatingAppAPI.Helpers
{
    public class UserParams
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

        public string CurrentUsername { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public string OrderBy { get; set; } = "lastActive";
    }
}