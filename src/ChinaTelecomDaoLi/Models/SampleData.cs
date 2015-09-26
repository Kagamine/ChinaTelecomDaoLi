using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Framework.DependencyInjection;

namespace ChinaTelecomDaoLi.Models
{
    public static class SampleData
    {
        public async static Task InitDB(IServiceProvider services)
        {
            var DB = services.GetRequiredService<DaoliContext>();
            var UserManager = services.GetRequiredService<UserManager<User>>();
            if (DB.Database != null && DB.Database.EnsureCreated())
            {
                var user = new User { UserName = "admin" };
                await UserManager.CreateAsync(user, "123456");
            }
        }
    }
}
