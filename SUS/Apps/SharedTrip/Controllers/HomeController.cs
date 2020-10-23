using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse Index()
        {
            if (!IsUserSignedIn())
            {
                return this.View();
            }
            return this.Redirect("/Trips/All");
        }
    }
}
