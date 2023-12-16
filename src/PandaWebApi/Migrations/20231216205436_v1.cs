using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pandatech.Crypto;
using PandaWebApi.Enums;

#nullable disable

namespace PandaWebApi.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        private static readonly IConfigurationRoot _configuration = ParameterConfigurations();
        
        private Argon2Id _argon2Id = new Argon2Id();
        
        private Aes256 aes256 = new Aes256(new Aes256Options()
        {
            Key = _configuration["Security:AESKey"]
        });
        private static IConfigurationRoot ParameterConfigurations()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: false)
                .Build();

            return configuration;
        }
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    force_password_change = table.Column<bool>(type: "boolean", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    signature_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_authentication_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_authenticated = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_authentication_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_authentication_history_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_tokens_expiration_date",
                table: "tokens",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "ix_tokens_signature_hash",
                table: "tokens",
                column: "signature_hash");

            migrationBuilder.CreateIndex(
                name: "ix_tokens_user_id",
                table: "tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_authentication_history_created_at",
                table: "user_authentication_history",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "ix_user_authentication_history_user_id",
                table: "user_authentication_history",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_full_name",
                table: "users",
                column: "full_name");

            migrationBuilder.CreateIndex(
                name: "ix_users_username",
                table: "users",
                column: "username",
                unique: true);
            
            // Custom user seed
            
            var username = _configuration["Security:SuperUser:Username"];
            var userPassword = _configuration["Security:SuperUser:Password"];
            
            var passwordHash = _argon2Id.HashPassword(userPassword);
            
            migrationBuilder.InsertData(
                table: "users",
                columns: new[]
                {
                    "username",
                    "full_name",
                    "password_hash",
                    "role",
                    "created_at",
                    "status",
                    "force_password_change",
                    "comment"
                },
                values: new object[]
                {
                    username,
                    "Super administrator",
                    passwordHash,
                    (int)Roles.SuperAdmin,
                    DateTime.UtcNow,
                    (int)Statuses.Active,
                    false,
                    "Seeded super user by the system. Please never delete this user."
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "user_authentication_history");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
