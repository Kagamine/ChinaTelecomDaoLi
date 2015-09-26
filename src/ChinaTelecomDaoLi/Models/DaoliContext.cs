using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace ChinaTelecomDaoLi.Models
{
    public class DaoliContext : IdentityDbContext<User>
    {
        public DbSet<CustomerDetail> CustomerDetails { get; set; }
        public DbSet<SameAreaRule> SameAreaRules { get; set; }
        public DbSet<SameAreaRuleDetail> SameAreaRuleDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerDetail>(e => 
            {
                e.Index(x => x.Account);
                //e.Index(x => x.AgentFee);
                //e.Index(x => x.Arrearage);
                e.Index(x => x.ContractorName);
                //e.Index(x => x.ContractorStruct);
                //e.Index(x => x.CurrentMonthBill);
                //e.Index(x => x.CustomerName);
                //e.Index(x => x.ImplementAddress);
                e.Index(x => x.ImportedTime);
                //e.Index(x => x.StandardAddress);
                e.Index(x => x.Status);
                //e.Index(x => x.Commission);
            });

            builder.Entity<SameAreaRuleDetail>(e =>
            {
                e.Index(x => x.Key);
            });
        }
    }
}
