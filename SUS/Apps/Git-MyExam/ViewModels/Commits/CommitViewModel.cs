using System;

namespace Git.ViewModels.Commits
{
    public class CommitViewModel
    {
        public string CommitId { get; set; }
        public string RepoName { get; set; }

        public string CommitDescription { get; set; }

        public DateTime CommitCreatedOn { get; set; }

        public string FromattedCommitTime => this.CommitCreatedOn.ToString("G");
    }
}
