using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movie247.Areas.Identity.Data;
using Movie247.Data;
using Movie247.Mail;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Movie247
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDbContext<Movie247Context>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("AppDbContext")));
            var mailsettings = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsettings);
            services.AddTransient<IEmailSender, SendMailService>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
            });
            //services.AddDefaultIdentity<Movie247User>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<Movie247Context>();
            services.AddDefaultIdentity<Movie247User>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<Movie247Context>();

            // show login path in the browser
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Movie247";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Redirect/AccessDenied";
                // options.SlidingExpiration = true;
                // options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            });
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero; // TimeSpan.FromSeconds(10)
            });
            // if admin lock a user, the user will be locked out of the system
            services.AddAuthentication()
            .AddFacebook(facebookOptions =>
            {

                IConfigurationSection facebookAuthNSection = Configuration.GetSection("Authentication:Facebook");
                facebookOptions.AppId = facebookAuthNSection["AppId"];
                facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];
                facebookOptions.CallbackPath = "/login-with-facebook";
            })
            .AddGoogle(googleOptions =>
            {
                IConfigurationSection googleAuthNSection = Configuration.GetSection("Authentication:Google");
                googleOptions.ClientId = googleAuthNSection["ClientId"];
                googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                googleOptions.CallbackPath = "/login-with-google";
            })
            .AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); // This one!
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "admin",
                    pattern: "admin/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(
                    name: "list of movie",
                    pattern: "{controller=Movie}/{action=List}/{id?}");
                endpoints.MapControllerRoute(
                    name: "details",
                    pattern: "{controller=Movie}/{action=Details}/{id?}");
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapControllerRoute(
                  name: "register",
                  pattern: "{controller=Account}/{action=Register}");
                endpoints.MapControllerRoute(
                    name: "confirm email",
                    pattern: "{controller=ConfirmEmail}/{action=Index}");
                endpoints.MapControllerRoute(
                  name: "profile",
                    pattern: "{controller=User}/{action=Profile}");
                endpoints.MapControllerRoute(
                name: "admin",
                  pattern: "{controller=admin}/{action=movies}/{id?}");
            });
        }
    }
}
