using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using MirrorOfErised.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;

using MirrorOfErised.models.Repos;
using MirrorOfErised.Models;
using MirrorOfErised.models.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using MirrorOfErised.Services;

namespace MirrorOfErised
{
    public class Startup
    {
        public Startup(IConfiguration configuration)  
        {
            Configuration = configuration;
        }

        public List<AuthenticationToken> tokens { get; set; }
        public List<Claim> claims { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //google toevoegen

            services.AddScoped<IAuthTokenRepo, AuthTokenRepo>();
            services.AddScoped<IUserEntryRepo, UserEntryRepo>();

            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");

                options.ClientId = "791529771236-h4e8h0m994t4skqn5ifgi50ct4cadbcn.apps.googleusercontent.com";
                options.ClientSecret = "E6fxpAJnvPuhk0ZFmDExJZ11";

                options.AccessType = "offline";

                options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
                options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
                options.SaveTokens = true;
                //scope for acces
                options.Scope.Add("https://www.googleapis.com/auth/calendar.readonly");
                options.Scope.Add("https://www.googleapis.com/auth/calendar.events.readonly");
                options.Scope.Add("https://www.googleapis.com/auth/assistant-sdk-prototype");
                
                options.Events.OnCreatingTicket = ctx =>
                {
                    /*UserManager<IdentityUser> usermanager = ctx.HttpContext.RequestServices.GetService<IAuthTokenRepo<AuthToken>>();
                                        usermanager.SetAuthenticationTokenAsync(),*/

                    tokens = ctx.Properties.GetTokens().ToList();
                    /*                    
                                        using (IAuthTokenRepo context = ) {

                                        }*/
                    claims = ctx.Identity.Claims.ToList();


                    tokens.Add(new AuthenticationToken()
                    {
                        Name = "Email",
                        Value = DateTime.UtcNow.ToString()


                    });
                    
                    
                    ctx.Properties.StoreTokens(tokens);

                    return Task.CompletedTask;
                };

                options.Events.OnTicketReceived = async ctx =>
                {

                    await ctx.HttpContext.RequestServices.GetService<IAuthTokenRepo>().Addtokens(tokens, claims);
                    //return Task.CompletedTask;

                };
            });


            services.AddHttpClient<GoogleCalendarAPI>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> usermgr, RoleManager<IdentityRole> rolemgr)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });


            ApplicationDbExtensions.SeedRoles(rolemgr).Wait();
            ApplicationDbExtensions.SeedUsers(usermgr, rolemgr).Wait();
        }
    }
}
