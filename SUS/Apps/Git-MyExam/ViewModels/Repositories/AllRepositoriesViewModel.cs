using System;

namespace Git.ViewModels.Repositories
{
    public class AllRepositoriesViewModel
    {
        public string RepoId { get; set; }
        public string RepoName { get; set; }

        public string Username { get; set; }

        public DateTime CreatenOn { get; set; }

        public string FormattedDate => this.CreatenOn.ToString("G");

        public int CommitsCount { get; set; }
    }
}
