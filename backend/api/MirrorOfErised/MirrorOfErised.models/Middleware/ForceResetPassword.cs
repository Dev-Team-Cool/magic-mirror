using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MirrorOfErised.models.Middleware
{
    public class ForceResetPasswordMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ForceResetPasswordMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<User> userManager)
        {
            string[] acceptedPaths = new string[1] {"/Identity/Account/ResetPassword"};
            var user = await userManager.GetUserAsync(context.User);
            if (user != null)
            {
                string requestPath = context.Request.Path;
                var userRoles = await userManager.GetRolesAsync(user);
                if (userRoles.Contains("Admin") && !user.ForcedPasswordReset
                    && Array.IndexOf(acceptedPaths, requestPath) == -1)
                {
                    string code = await userManager.GeneratePasswordResetTokenAsync(user);
                    code = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));
                    context.Response.Redirect($"/Identity/Account/ResetPassword?code={code}");
                }
            }
            await _next(context);
        }
    }
    
    public static class UseForceResetPasswordMiddlewareExtension
    {
        public static IApplicationBuilder UseForceResetPassword(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ForceResetPasswordMiddleware>();
        }
    }
}