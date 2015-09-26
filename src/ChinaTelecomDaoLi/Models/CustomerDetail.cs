using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ChinaTelecomDaoLi.Models
{
    public enum Status
    {
        在用,
        单向欠停,
        双向欠停,
        欠费拆机,
        用户报停,
        用户拆机,
        装机未竣工退单,
        违章停机,
        预拆机
    }

    public class CustomerDetail
    {
        public Guid Id { get; set; }

        public DateTime ImportedTime { get; set; }
        
        [MaxLength(128)]
        public string Account { get; set; }

        public Status Status { get; set; }

        [MaxLength(32)]
        public string CustomerName { get; set; }

        [MaxLength(32)]
        public string ContractorName { get; set; }

        [MaxLength(128)]
        public string ContractorStruct { get; set; }
        
        public double CurrentMonthBill { get; set; }

        public double AgentFee { get; set; }

        public double Arrearage { get; set; }
        
        public double Commission { get; set; }

        [MaxLength(2048)]
        public string ImplementAddress { get; set; }

        [MaxLength(2048)]
        public string StandardAddress { get; set; }
    }
}
