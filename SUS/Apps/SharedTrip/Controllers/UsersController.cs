using SharedTrip.Services;
using SharedTrip.ViewModels.User;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(LoginUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            var userId = this.usersService.GetUserId(input);

            if(userId == null)
            {
                return this.Error("Invalid username or password.");
            }
            this.SignIn(userId);
            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(CreateUserInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }
            if (string.IsNullOrEmpty(input.Username)
                || input.Username.Length <5 
                || input.Username.Length > 20)
            {
                return this.Error("Invalid username.");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("This username is already taken.");
            }

            if(string.IsNullOrEmpty(input.Email)
                || ! new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Invalid email.");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("This email is already taken.");
            }

            if(string.IsNullOrEmpty(input.Password)
                || input.Password.Length < 6
                || input.Password.Length > 20)
            {
                return this.Error("Invalid password.");
            }

            if(input.Password != input.ConfirmPassword)
            {
                return this.Error("Both passwords do not match.");
            }

            this.usersService.CreateUser(input);
            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }
            this.SignOut();
            return this.Redirect("/");
        }
    }
}
