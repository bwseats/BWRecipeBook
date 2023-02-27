﻿using BWRecipeBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BWRecipeBook.Data
{
    public static class DataUtility
    {
        public static DateTime GetPostGresDate(DateTime datetime)
        {
            return DateTime.SpecifyKind(datetime, DateTimeKind.Utc);
        }

        public static string GetConnectionString(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);

        }

        private static string BuildConnectionString(string databaseUrl)
        {
            Uri databaseUri = new(databaseUrl);
            string[] userInfo = databaseUri.UserInfo.Split(':');
            NpgsqlConnectionStringBuilder builder = new()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userManagerSvc = svcProvider.GetRequiredService<UserManager<RBUser>>();

            await dbContextSvc.Database.MigrateAsync();

            await SeedDemoUserAsync(userManagerSvc);
        }

        private static async Task SeedDemoUserAsync(UserManager<RBUser> userManager)
        {
            RBUser demoUser = new RBUser()
            {
                UserName = "demo@recipebook.com",
                Email = "demo@recipebook.com",
                FirstName = "Demo",
                LastName = "User",
                EmailConfirmed = true
            };

            try
            {
                RBUser? user = await userManager.FindByEmailAsync(demoUser.Email);

                if (user == null)
                {
                    await userManager.CreateAsync(demoUser, "Demo123!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("!------------ I AM ERROR. ------------!");
                Console.WriteLine("Error seeding demo login user.");
                Console.WriteLine(ex.Message);
                Console.WriteLine("!-------------------------------------!");

                throw;
            }
        }
    }
}
