using System;
using System.Linq;
using System.Threading.Tasks;
using IronPython.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

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
            string[] acceptedPaths = new string[3] {"/UserEntry/Create", "/UserEntry/Upload", "/Identity/Account/Logout"};
            var user = await userManager.GetUserAsync(context.User);
            if (user != null)
            {
                string requestedPath = context.Request.Path;
                if (!user.HasCompletedSignUp && Array.IndexOf(acceptedPaths, requestedPath) == -1)
                { 
                    context.Response.Redirect("/UserEntry/Create");
                    return;
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