using QA.Entities.Session_Entities;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace QA.Controllers
{
    public class BaseController : Controller
    {
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            try
            {
                string cultureName = null;

                // Attempt to read the culture cookie from Request
                HttpCookie cultureCookie = Request.Cookies["_culture"];
                if (cultureCookie != null)
                    cultureName = cultureCookie.Value;
                else
                    cultureName = (Request.UserLanguages != null && Request.UserLanguages.Length > 0) ?
                            Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
                            null;

                // Validate culture name
                //cultureName = "en-us";
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

                // Modify current thread's cultures            
                //Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
                //Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

                //cultureName = "ar-SA";
                //Set language and culture to Arabic  
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
                //But independent of language, keep datetime format same
                DateTimeFormatInfo englishDateTimeFormat = new CultureInfo("ca").DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = englishDateTimeFormat;

                return base.BeginExecuteCore(callback, state);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}