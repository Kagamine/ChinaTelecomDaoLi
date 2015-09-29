using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using ChinaTelecomDaoLi.Models;

namespace ChinaTelecomDaoLi.Controllers
{
    [Authorize]
    public class RuleController : BaseController
    {
        [HttpGet]
        public IActionResult Address()
        {
            var ret = DB.SameAreaRules
                .Include(x => x.Details)
                .ToList();
            return View(ret);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Address(string addresses)
        {
            var tmp = addresses.Split('\n');
            if (tmp.Count() > 0)
            {
                var rule = new SameAreaRule();
                DB.SameAreaRules.Add(rule);
                foreach (var s in tmp)
                {
                    DB.SameAreaRuleDetails.Add(new SameAreaRuleDetail
                    {
                        RuleId = rule.Id,
                        Key = s.Trim()
                    });
                }
                DB.SaveChanges();
            }
            return RedirectToAction("Address", "Rule");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAddress(long id)
        {
            var addr = DB.SameAreaRules
                .Include(x => x.Details)
                .Where(x => x.Id == id)
                .SingleOrDefault();
            if (addr == null)
                return Error(404);
            foreach (var x in addr.Details)
                DB.SameAreaRuleDetails.Remove(x);
            DB.SameAreaRules.Remove(addr);
            DB.SaveChanges();
            return RedirectToAction("Address", "Rule");
        }
    }
}
