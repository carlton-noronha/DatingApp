using DatingAppAPI.Entities;
using DatingAppAPI.Extentions;
using DatingAppAPI.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingAppAPI.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ActionExecutedContext resultContext = await next();
            if (resultContext.HttpContext.User.Identity.IsAuthenticated) {
                int id = resultContext.HttpContext.User.GetUserID();
                IUserRepository userRepository = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                AppUser user = await userRepository.GetUserByIdAsync(id);
                user.LastActive = DateTime.UtcNow;
                await userRepository.SaveAllAsync();
            }
        }
    }
}