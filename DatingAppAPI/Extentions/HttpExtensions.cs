using System.Text.Json;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Extentions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header) {
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}