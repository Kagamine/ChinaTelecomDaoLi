using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinaTelecomDaoLi.Models
{
    public class SameAreaRule
    {
        public long Id { get; set; }

        public virtual ICollection<SameAreaRuleDetail> Details { get; set; } = new List<SameAreaRuleDetail>();
    }
}
