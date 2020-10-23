using Microsoft.EntityFrameworkCore.Internal;
using SharedTrip.Data;
using SharedTrip.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
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
        public string GetUserById(string username, string password)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username
            && x.Password == ComputeHash(password));

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

        public void RegisterUser(RegisterUserInputModel input)
        {
            this.db.Users.Add(new User
            {
                Username = input.Username,
                Email = input.Email,
                Password = ComputeHash(input.Password)
            });
            this.db.SaveChanges();
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
