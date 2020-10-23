using SharedTrip.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface IUsersService
    {
        void RegisterUser(RegisterUserInputModel input);
        string GetUserById(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
