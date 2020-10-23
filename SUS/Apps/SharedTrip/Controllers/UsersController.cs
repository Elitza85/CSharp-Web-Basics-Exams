using SharedTrip.Services;
using SharedTrip.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SharedTrip.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public HttpResponse Login()
        {
            if (IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]

        public HttpResponse Login(string username, string password)
        {
            if (IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserById(username, password);
            if(userId == null)
            {
                return this.Error("Invalid username or password.");
            }
            this.SignIn(userId);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            if (IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(RegisterUserInputModel input)
        {
            if (IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(input.Username)
                || input.Username.Length < 5
                || input.Username.Length > 20)
            {
                return this.Error("Username is required and should be between 5 and 20 characters.");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("This username is already taken.");
            }

            if(string.IsNullOrEmpty(input.Email)
                || !new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Email is required and should be in valid format.");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("This email is already taken.");
            }

            if (string.IsNullOrEmpty(input.Password)
                || input.Password.Length < 6 || input.Password.Length > 20)
            {
                return this.Error("Password is required and should be between 6 and 20 characters.");
            }

            if(input.Password != input.ConfirmPassword)
            {
                return this.Error("Both password do not match.");
            }

            this.usersService.RegisterUser(input);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            this.SignOut();
            return this.Redirect("/");
        }
    }
}
