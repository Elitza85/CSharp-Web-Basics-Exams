using Microsoft.EntityFrameworkCore.Internal;
using SharedTrip.Data;
using SharedTrip.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharedTrip.Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void CreateUser(CreateUserInputModel input)
        {
            this.db.Users.Add(new User
            {
                Username = input.Username,
                Email = input.Email,
                Password = ComputeHash(input.Password)
            });
            db.SaveChanges();
        }

        public string GetUserId(LoginUserInputModel input)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == input.Username
            && x.Password == ComputeHash(input.Password));

            return user?.Id;
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(x => x.Email == email);
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db.Users.Any(x => x.Username == username);
        }

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            // Convert to text
            // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
}
