using Suls.Services;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Controllers
{
    public class ProblemsController :Controller
    {
        private readonly IProblemsService problemsService;

        public ProblemsController(IProblemsService problemsService)
        {
            this.problemsService = problemsService;
        }
        public HttpResponse Create()
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(string name, ushort points)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            if (string.IsNullOrEmpty(name) || name.Length < 5 || name.Length > 20)
            {
                return this.Error("Invalid problem name.");
            }

            if(points<50 || points > 300)
            {
                return this.Error("Invalid problem points.");
            }

            this.problemsService.Create(name, points);
            return this.Redirect("/");
        }


        public HttpResponse Details(string id)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var viewModel =this.problemsService.GetById(id);
            return this.View(viewModel);
        }
    }
}
