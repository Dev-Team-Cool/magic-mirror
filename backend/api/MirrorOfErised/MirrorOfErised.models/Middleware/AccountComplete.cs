using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models.Middleware
{
    public class AccountCompleteMiddleware
    {
        private readonly RequestDelegate _next;
        
        public AccountCompleteMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user != null)
            {
                if (context.Request.Path != "/Identity/Account/ConfirmEmail" && context.Request.Path != "/UserEntry/Create")
                {
                    if (!user.EmailConfirmed) context.Response.Redirect("/Identity/Account/ConfirmEmail");
                    if (!user.HasCompletedSignUp) context.Response.Redirect("/UserEntry/Create");
                }
            }
            await _next(context);
        }
    }
    
    public static class UseAccountCompleteMiddlewareExtension
    {
        public static IApplicationBuilder UseAccountComplete(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccountCompleteMiddleware>();
        }
    }
}