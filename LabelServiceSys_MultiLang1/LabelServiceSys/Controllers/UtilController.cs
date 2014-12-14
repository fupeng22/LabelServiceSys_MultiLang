using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LabelServiceSys.Filter;

namespace LabelServiceSys.Controllers
{
     [ErrorAttribute]
    public class UtilController : Controller
    {
        //
        // GET: /Util/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string getDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
