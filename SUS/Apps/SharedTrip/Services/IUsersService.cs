using SharedTrip.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedTrip.Services
{
    public interface IUsersService
    {
        string GetUserId(LoginUserInputModel input);

        void CreateUser(CreateUserInputModel input);

        bool IsEmailAvailable(string email);

        bool IsUsernameAvailable(string username);
    }
}
