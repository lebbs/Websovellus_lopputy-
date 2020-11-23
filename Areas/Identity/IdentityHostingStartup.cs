using System;
using DataViewer.Areas.Identity.Data;
using DataViewer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(DataViewer.Areas.Identity.IdentityHostingStartup))]
namespace DataViewer.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DataViewerContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DataViewerContextConnection")));

                services.AddDefaultIdentity<DataViewerUser>()
                    .AddEntityFrameworkStores<DataViewerContext>();
            });
        }
    }
}