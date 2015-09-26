using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Dnx.Runtime;
using Microsoft.Data.Entity;
using ChinaTelecomDaoLi.Models;

namespace ChinaTelecomDaoLi.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        public IActionResult Index(string ContractorName, Status? Status, string Address)
        {
            IEnumerable<CustomerDetail> ret = DB.CustomerDetails.AsNoTracking();
            if (!string.IsNullOrEmpty(ContractorName))
                ret = ret.Where(x => x.ContractorName == ContractorName);
            if (Status.HasValue)
                ret = ret.Where(x => x.Status == Status);
            if (!string.IsNullOrEmpty(Address))
            {
                var keywords = DB.SameAreaRuleDetails
                    .Include(x => x.Rule)
                    .ThenInclude(x => x.Details)
                    .Where(x => x.Key.Contains(Address))
                    .Select(x => x.Key)
                    .ToList();
                
                ret = ret.ToList().Where(x => 
                {
                    if (x.ImplementAddress.Contains(Address))
                        return true;
                    foreach (var y in keywords)
                        if (x.ImplementAddress.Contains(y))
                            return true;
                    return false;
                });
            }
            ViewBag.Statuses = ret.Select(x => x.Status).Distinct().ToList();
            ViewBag.ContractorNames = ret.Select(x => x.ContractorName).Distinct().ToList();
            return PagedView(ret);
        }
        
        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Import(IFormFile file)
        {
            var env = Resolver.GetService<IApplicationEnvironment>();
            if (!Directory.Exists(env.ApplicationBasePath + "/tmp"))
                Directory.CreateDirectory(env.ApplicationBasePath + "/tmp");
            var fname = env.ApplicationBasePath + "/tmp/" + Guid.NewGuid() + Path.GetExtension(file.GetFileName());
            file.SaveAs(fname);
            string connStr;
            if (Path.GetExtension(fname) == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fname + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + fname + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            Task.Factory.StartNew(() => 
            {
                long i = 0;
                var conn = new OleDbConnection(connStr);
                conn.Open();
                var cmd = new OleDbCommand("select * from [Sheet1$]", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    var flag = reader.Read();
                    if (flag)
                    {
                        while (reader.Read())
                        {
                            i++;
                            if (i % 1000 == 0)
                            {
                                Console.WriteLine("已经导入" + i +"条记录。");
                            }
                            try
                            {
                                Status status;
                                if (reader[1].ToString() == "欠停(单向)")
                                    status = Status.单向欠停;
                                else if (reader[1].ToString() == "欠停(双向)")
                                    status = Status.双向欠停;
                                else
                                    status = (Status)Enum.Parse(typeof(Status), reader[1].ToString());
                                if (DB.CustomerDetails.Any(x => x.Account == reader[0].ToString()))
                                {
                                    var detail = DB.CustomerDetails.Where(x => x.Account == reader[0].ToString()).Single();
                                    detail.Status = status;
                                    detail.CustomerName = reader[2].ToString();
                                    detail.ContractorName = reader[3].ToString();
                                    detail.ContractorStruct = reader[4].ToString();
                                    detail.CurrentMonthBill = reader[5] == DBNull.Value ? 0 : Convert.ToDouble(reader[5]);
                                    detail.AgentFee = reader[7] == DBNull.Value ? 0 : Convert.ToDouble(reader[7]);
                                    detail.Commission = reader[8] == DBNull.Value ? 0 : Convert.ToDouble(reader[8]);
                                    detail.Arrearage = reader[9] == DBNull.Value ? 0 : Convert.ToDouble(reader[9]);
                                    detail.ImplementAddress = reader[10].ToString();
                                    detail.StandardAddress = reader[11].ToString();
                                }
                                else
                                {
                                    DB.CustomerDetails.Add(new CustomerDetail
                                    {
                                        Account = reader[0].ToString(),
                                        Status = status,
                                        CustomerName = reader[2].ToString(),
                                        ContractorName = reader[3].ToString(),
                                        ContractorStruct = reader[4].ToString(),
                                        CurrentMonthBill = reader[5] == DBNull.Value ? 0 : Convert.ToDouble(reader[5]),
                                        AgentFee = reader[7] == DBNull.Value ? 0 : Convert.ToDouble(reader[7]),
                                        Commission = reader[8] == DBNull.Value ? 0 : Convert.ToDouble(reader[8]),
                                        Arrearage = reader[9] == DBNull.Value ? 0 : Convert.ToDouble(reader[9]),
                                        ImplementAddress = reader[10].ToString(),
                                        StandardAddress = reader[11].ToString()
                                    });
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error #" + i);
                            }
                        }
                    }
                }
                DB.SaveChanges();
                conn.Close();
            });
            return RedirectToAction("Importing", "Customer");
        }

        public IActionResult Importing()
        {
            return View();
        }
    }
}
