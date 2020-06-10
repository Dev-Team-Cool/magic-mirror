using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;

using MirrorOfErised.models.Repos;
using MirrorOfErised.models.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using MirrorOfErised.models;
using MirrorOfErised.models.Middleware;
using MirrorOfErised.models.Services;
using MirrorOfErised.Services;

namespace MirrorOfErised
{
    public class Startup
    {
        public Startup(IConfiguration configuration)  
        {
            Configuration = configuration;
        }

        private List<AuthenticationToken> tokens { get; set; }
        private List<Claim> claims { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });
            
            services.AddDefaultIdentity<User>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IAuthTokenRepo, AuthTokenRepo>();
            services.AddScoped<IUserEntryRepo, UserEntryRepo>();
            services.AddScoped<IUserSettingsRepo, UserSettingsRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ITrainJobRepo, TrainJobRepo>();
            services.AddScoped<PythonRunner>();
            services.AddScoped<ITrainJobService, TrainJobService>();

            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");

                options.ClientId = Environment.GetEnvironmentVariable("CLIENTID");
                options.ClientSecret = Environment.GetEnvironmentVariable("CLIENTSECRET");

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
                    tokens = ctx.Properties.GetTokens().ToList();
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
                };
            });


            services.AddHttpClient<GoogleCalendarService>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> usermgr, RoleManager<IdentityRole> rolemgr)
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

            app.UseAccountComplete();

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
