using Suls.Services;
using Suls.ViewModels;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suls.Controllers
{
    public class SubmissionsController : Controller
    {
        private readonly IProblemsService problemsService;
        private readonly ISubmissionsService submissionsService;

        public SubmissionsController(IProblemsService problemsService,
            ISubmissionsService submissionsService)
        {;
            this.problemsService = problemsService;
            this.submissionsService = submissionsService;
        }
        public HttpResponse Create(string id)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            var viewModel = new CreateViewModel
            {
                ProblemId = id,
                Name = this.problemsService.GetNameById(id)
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(string problemId, string code)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrEmpty(code) || code.Length < 30 || code.Length > 800)
            {
                return this.Error("Invalid submission.");
            }
            var userId = this.GetUserId();
            this.submissionsService.Create(problemId, userId, code);
            return this.Redirect("/");
        }

        public HttpResponse Delete(string id)
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.submissionsService.Delete(id);
            return this.Redirect("/");
        }
    }
}
