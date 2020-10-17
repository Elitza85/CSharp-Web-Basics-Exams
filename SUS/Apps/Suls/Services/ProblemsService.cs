using Suls.Data;
using Suls.ViewModels;
using Suls.ViewModels.Problems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Suls.Services
{
    public class ProblemsService : IProblemsService
    {
        private readonly ApplicationDbContext db;

        public ProblemsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string name, ushort points)
        {
            this.db.Problems.Add(new Problem
            {
                Name = name,
                Points = points
            });
            this.db.SaveChanges();
        }

        public IEnumerable<HomePageProblemViewModel> GetAll()
        {
            return this.db.Problems.Select(x => new HomePageProblemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Submissions.Count()
            }).ToList();
        }


        public string GetNameById(string id)
        {
            return this.db.Problems.Where(x => x.Id == id)
                .Select(x=>x.Name).FirstOrDefault();
        }
        public ProblemViewModel GetById(string id)
        {
            return this.db.Problems.Where(x => x.Id == id)
                .Select(x=>new ProblemViewModel
                {
                    Name = x.Name,
                    Submissions = x.Submissions.Select(y=> new SubmissionViewModel
                    {
                        Username = y.User.Username,
                        CreatedOn= y.CreatedOn,
                        SubmissionId = y.Id,
                        AchievedResult = y.AchievedResult,
                        MaxPoints = x.Points
                    })
                }).FirstOrDefault();
        }
    }
}
