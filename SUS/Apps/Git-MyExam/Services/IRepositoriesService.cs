using Git.ViewModels.Repositories;
using System.Collections.Generic;

namespace Git.Services
{
    public interface IRepositoriesService
    {
        void Create(string name, string type, string userId);

        IEnumerable<AllRepositoriesViewModel> GetAllRepos();
    }
}
