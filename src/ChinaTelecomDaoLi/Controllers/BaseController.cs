using Microsoft.AspNet.Mvc;
using ChinaTelecomDaoLi.Models;

namespace ChinaTelecomDaoLi.Controllers
{
    public class BaseController : BaseController<User, DaoliContext, string>
    {

    }
}
