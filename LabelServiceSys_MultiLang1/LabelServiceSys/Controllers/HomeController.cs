using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabelServiceSys.Filter;

namespace LabelServiceSys.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        [LoginValidate]
        public ActionResult Index()
        {
            return View();
        }

    }
}
