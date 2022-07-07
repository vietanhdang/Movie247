using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Movie247.Areas.Identity.Data;
using Movie247.Data;

[assembly: HostingStartup(typeof(Movie247.Areas.Identity.IdentityHostingStartup))]
namespace Movie247.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<Movie247Context>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AppDbContext")));

                services.AddDefaultIdentity<Movie247User>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<Movie247Context>();
            });
        }
    }
}