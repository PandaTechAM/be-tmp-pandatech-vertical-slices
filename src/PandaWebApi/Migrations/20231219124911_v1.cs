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
            migrationBuilder.EnsureSchema(
                name: "hangfire");

            migrationBuilder.CreateTable(
                name: "hangfire_counter",
                schema: "hangfire",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false),
                    expire_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_counter", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_hash",
                schema: "hangfire",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    field = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true),
                    expire_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_hash", x => new { x.key, x.field });
                });

            migrationBuilder.CreateTable(
                name: "hangfire_list",
                schema: "hangfire",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true),
                    expire_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_list", x => new { x.key, x.position });
                });

            migrationBuilder.CreateTable(
                name: "hangfire_lock",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    acquired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_lock", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_server",
                schema: "hangfire",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    heartbeat = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    worker_count = table.Column<int>(type: "integer", nullable: false),
                    queues = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_server", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_set",
                schema: "hangfire",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    value = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    score = table.Column<double>(type: "double precision", nullable: false),
                    expire_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_set", x => new { x.key, x.value });
                });

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

            migrationBuilder.CreateTable(
                name: "hangfire_job",
                schema: "hangfire",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    state_id = table.Column<long>(type: "bigint", nullable: true),
                    state_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    expire_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    invocation_data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_job", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_job_parameter",
                schema: "hangfire",
                columns: table => new
                {
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_job_parameter", x => new { x.job_id, x.name });
                    table.ForeignKey(
                        name: "fk_hangfire_job_parameter_hangfire_job_job_id",
                        column: x => x.job_id,
                        principalSchema: "hangfire",
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_queued_job",
                schema: "hangfire",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    queue = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    fetched_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_queued_job", x => x.id);
                    table.ForeignKey(
                        name: "fk_hangfire_queued_job_hangfire_job_job_id",
                        column: x => x.job_id,
                        principalSchema: "hangfire",
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_state",
                schema: "hangfire",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    job_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_hangfire_state", x => x.id);
                    table.ForeignKey(
                        name: "fk_hangfire_state_hangfire_job_job_id",
                        column: x => x.job_id,
                        principalSchema: "hangfire",
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_counter_expire_at",
                schema: "hangfire",
                table: "hangfire_counter",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_counter_key_value",
                schema: "hangfire",
                table: "hangfire_counter",
                columns: new[] { "key", "value" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_hash_expire_at",
                schema: "hangfire",
                table: "hangfire_hash",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_expire_at",
                schema: "hangfire",
                table: "hangfire_job",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_state_id",
                schema: "hangfire",
                table: "hangfire_job",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_state_name",
                schema: "hangfire",
                table: "hangfire_job",
                column: "state_name");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_list_expire_at",
                schema: "hangfire",
                table: "hangfire_list",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_queued_job_job_id",
                schema: "hangfire",
                table: "hangfire_queued_job",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_queued_job_queue_fetched_at",
                schema: "hangfire",
                table: "hangfire_queued_job",
                columns: new[] { "queue", "fetched_at" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_server_heartbeat",
                schema: "hangfire",
                table: "hangfire_server",
                column: "heartbeat");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_set_expire_at",
                schema: "hangfire",
                table: "hangfire_set",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_set_key_score",
                schema: "hangfire",
                table: "hangfire_set",
                columns: new[] { "key", "score" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_state_job_id",
                schema: "hangfire",
                table: "hangfire_state",
                column: "job_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_hangfire_job_hangfire_state_hangfire_state_id",
                schema: "hangfire",
                table: "hangfire_job",
                column: "state_id",
                principalSchema: "hangfire",
                principalTable: "hangfire_state",
                principalColumn: "id");
            
            //Seeded data
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
            migrationBuilder.DropForeignKey(
                name: "fk_hangfire_job_hangfire_state_hangfire_state_id",
                schema: "hangfire",
                table: "hangfire_job");

            migrationBuilder.DropTable(
                name: "hangfire_counter",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_hash",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_job_parameter",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_list",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_lock");

            migrationBuilder.DropTable(
                name: "hangfire_queued_job",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_server",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_set",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "tokens");

            migrationBuilder.DropTable(
                name: "user_authentication_history");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "hangfire_state",
                schema: "hangfire");

            migrationBuilder.DropTable(
                name: "hangfire_job",
                schema: "hangfire");
        }
    }
}
