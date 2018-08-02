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
        private readonly RoleManager<Role> _roleManager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public async void SeedUsers()
        {
            if (!_userManager.Users.Any())
            {
                //seed users
                var userData = System.IO.File.ReadAllText("Data\\UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                //create some roles 
                var roles = new List<Role>
                {
                    new Role{Name="Member"},
                    new Role{Name="Admin"},
                    new Role{Name="Moderator"},
                    new Role{Name="VIP"}
                };

                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(role);
                }

                foreach (var user in users)
                {
                    await _userManager.CreateAsync(user, "password");
                    await _userManager.AddToRoleAsync(user, "Member");

                    //Commented after adding .NET Core authorization
                    // //create the password hash
                    // byte[] passwordHash, passwordSalt;
                    // CreatePasswordHash("password", out passwordHash, out passwordSalt);

                    //Commented after adding .NET Core authorization
                    // user.PasswordHash=passwordHash;
                    // user.PasswordSalt=passwordSalt;
                    // user.UserName = user.UserName.ToLower();                   
                }

                //create admin user
                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var result = await _userManager.CreateAsync(adminUser, "password");

                if (result.Succeeded)
                {
                    var admin = await _userManager.FindByNameAsync("Admin");
                    await _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
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