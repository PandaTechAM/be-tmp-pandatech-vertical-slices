using System.Collections;
using Pandatech.Crypto;
using PandaWebApi.Contexts;
using PandaWebApi.Enums;
using PandaWebApi.Models;

namespace PandaWebApi.Extensions;

public static class SeederExtension
{
    public static WebApplication SeedSystemUser(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<PostgresContext>();
            var configuration = services.GetRequiredService<IConfiguration>();
            var argon2Id = services.GetRequiredService<Argon2Id>();

            var username = configuration["Security:SuperUser:Username"];
            ValidateConfiguration(username, "SuperUser:Username");

            var normalizedUsername = username!.ToLowerInvariant();
            var existingUsers = context.Users
                .Where(u => u.Username == normalizedUsername || u.Role == Roles.SuperAdmin)
                .ToList();

            ValidateSuperUserUniqueness(existingUsers);

            if (existingUsers.Count == 1)
            {
                return app;
            }

            var userPassword = configuration["Security:SuperUser:Password"];
            ValidateConfiguration(userPassword, "SuperUser:Password");

            var passwordHash = argon2Id.HashPassword(userPassword!);
            var currentTime = DateTime.UtcNow;

            var newUser = CreateNewUser(normalizedUsername, passwordHash, currentTime);
            context.Users.Add(newUser);
            context.SaveChanges();

            return app;
        }

        private static void ValidateConfiguration(string? configValue, string configName)
        {
            if (string.IsNullOrWhiteSpace(configValue))
            {
                throw new ArgumentException($"{configName} is not set in appsettings.json");
            }
        }

        private static void ValidateSuperUserUniqueness(ICollection users)
        {
            if (users.Count > 1)
            {
                throw new InvalidOperationException("There are multiple super users in the database.");
            }
        }

        private static User CreateNewUser(string username, byte[] passwordHash, DateTime currentTime)
        {
            return new User
            {
                FullName = "System",
                PasswordHash = passwordHash,
                Username = username,
                Role = Roles.SuperAdmin,
                ForcePasswordChange = false,
                Status = Statuses.Active,
                CreatedAt = currentTime,
                UpdatedAt = currentTime,
                Comment = "Seeded user, please do not delete"
            };
        }
}