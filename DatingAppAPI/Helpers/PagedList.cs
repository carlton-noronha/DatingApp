using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Helpers
{
    public class PagedList<T>: List<T>
    {
        public PagedList(IEnumerable<T> items, long count, int pageNumber, int pageSize)
        {
            this.CurrentPage = pageNumber;
            this.TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            this.PageSize = pageSize;
            this.TotalCount = count;
            AddRange(items);
        }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize) {
            long count = await source.CountAsync();
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}