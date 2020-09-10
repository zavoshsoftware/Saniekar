using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infrastructure
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        protected override System.IAsyncResult BeginExecuteCore(System.AsyncCallback callback, object state)
        {
            System.Globalization.CultureInfo oCultureInfo =
                new System.Globalization.CultureInfo("fa-IR");

            System.Threading.Thread.CurrentThread.CurrentCulture = oCultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = oCultureInfo;

            return base.BeginExecuteCore(callback, state);
        }
    }
}