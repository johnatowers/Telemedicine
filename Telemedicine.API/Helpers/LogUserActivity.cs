using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Telemedicine.API.Data;

namespace Telemedicine.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        // Waiting until an action has been completed and then updating users LastAction property
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // type of action executed context
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContext.HttpContext.RequestServices.GetService<ITelemedRepository>();
            var user = await repo.getUser(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();
        }
    }
}