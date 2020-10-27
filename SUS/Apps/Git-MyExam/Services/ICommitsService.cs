using Git.ViewModels.Commits;
using System.Collections.Generic;

namespace Git.Services
{
    public interface ICommitsService
    {
        void CreateCommit(string id, string description, string userId);

        CreateCommitViewModel GetCreateForm(string id);

        IEnumerable<CommitViewModel> GetOwnCommits(string userId);

        void DeleteCommit(string id);

        bool CanDeleteCommit(string commitId, string userId);
    }
}
