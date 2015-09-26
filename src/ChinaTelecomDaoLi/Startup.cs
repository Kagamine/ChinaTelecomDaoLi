using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using ChinaTelecomDaoLi.Models;

namespace ChinaTelecomDaoLi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var _services = services.BuildServiceProvider();
            var appEnv = _services.GetRequiredService<IApplicationEnvironment>();

            services.AddEntityFramework()
                        .AddSqlite()
                        .AddDbContext<DaoliContext>(x => x.UseSqlite($"data source={appEnv.ApplicationBasePath}/Database/chinatelecom.db;"));

            services.AddIdentity<User, IdentityRole>(x =>
            {
                x.Password.RequiredLength = 4;
                x.Password.RequireDigit = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireNonLetterOrDigit = false;
                x.Password.RequireUppercase = false;
                x.User.RequireUniqueEmail = false;
                x.User.AllowedUserNameCharacters = null;
            })
                .AddEntityFrameworkStores<DaoliContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public async void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.MinimumLevel = LogLevel.Information;
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseIdentity();
            app.UseStaticFiles();

            app.UseMvc(router =>
            {
                router.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            await SampleData.InitDB(app.ApplicationServices);
        }
    }
}
