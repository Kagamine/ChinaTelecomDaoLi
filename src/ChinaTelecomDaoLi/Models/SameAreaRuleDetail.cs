using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChinaTelecomDaoLi.Models
{
    public class SameAreaRuleDetail
    {
        public long Id { get; set; }

        [MaxLength(32)]
        public string Key { get; set; }

        [ForeignKey("Rule")]
        public long RuleId { get; set; }

        public SameAreaRule Rule { get; set; }
    }
}
