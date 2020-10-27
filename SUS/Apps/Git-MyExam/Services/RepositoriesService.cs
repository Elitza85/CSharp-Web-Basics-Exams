using Git.Data;
using Git.ViewModels.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class RepositoriesService : IRepositoriesService
    {
        private readonly ApplicationDbContext db;

        public RepositoriesService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Create(string name, string type, string userId)
        {
            var repo = new Repository
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
                IsPublic = type == "Public",
                OwnerId = userId,
            };
            this.db.Repositories.Add(repo);
            this.db.SaveChanges();
        }

        public IEnumerable<AllRepositoriesViewModel> GetAllRepos()
        {
            return this.db.Repositories
                .Where(x => x.IsPublic)
                .Select(r => new AllRepositoriesViewModel
                {
                    RepoId = r.Id,
                    RepoName = r.Name,
                    CreatenOn = r.CreatedOn,
                    Username = r.Owner.Username,
                    CommitsCount = r.Commits.Count()
                }).ToList();
        }
    }
}
