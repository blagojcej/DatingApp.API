using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        public Seed(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                //seed users
                var userData = System.IO.File.ReadAllText("Data\\UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "password");                    

                    //Commented after adding .NET Core authorization
                    // //create the password hash
                    // byte[] passwordHash, passwordSalt;
                    // CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    //Commented after adding .NET Core authorization
                    // user.PasswordHash=passwordHash;
                    // user.PasswordSalt=passwordSalt;
                    // user.UserName = user.UserName.ToLower();                   
                }                
            }
        }

        //Commented after adding .NET Core authorization
        /*
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        */
    }
}