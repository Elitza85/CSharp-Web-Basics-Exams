using Git.Data;
using Git.ViewModels.Commits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Git.Services
{
    public class CommitsService : ICommitsService
    {
        private readonly ApplicationDbContext db;

        public CommitsService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void CreateCommit(string id, string description, string userId)
        {
            var commit = new Commit
            {
                Description = description,
                CreatedOn = DateTime.UtcNow,
                CreatorId = userId,
                RepositoryId = id
            };
            this.db.Commits.Add(commit);
            this.db.SaveChanges();
        }


        public CreateCommitViewModel GetCreateForm(string id)
        {
             return this.db.Repositories
                .Where(x => x.Id == id)
                .Select(x => new CreateCommitViewModel
                {
                    RepoName = x.Name,
                    RepoId = id
                }).FirstOrDefault();
        }

        public IEnumerable<CommitViewModel> GetOwnCommits(string userId)
        {
            return this.db.Commits
                .Where(x => x.CreatorId == userId)
                .Select(x => new CommitViewModel
                {
                    CommitId = x.Id,
                    RepoName = x.Repository.Name,
                    CommitDescription = x.Description,
                    CommitCreatedOn = x.CreatedOn
                }).ToList();
        }
        public void DeleteCommit(string id)
        {
            var commit = this.db.Commits.First(x => x.Id == id);
            this.db.Commits.Remove(commit);
            this.db.SaveChanges();
        }

        public bool CanDeleteCommit(string commitId, string userId)
        {
            return this.db.Commits.Any(x => x.Id == commitId
            && x.CreatorId == userId);
        }
    }
}
