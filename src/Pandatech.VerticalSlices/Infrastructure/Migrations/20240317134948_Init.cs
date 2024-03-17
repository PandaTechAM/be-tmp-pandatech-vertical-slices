using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Pandatech.VerticalSlices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hangfire_counter",
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
                    status = table.Column<int>(type: "integer", nullable: false),
                    force_password_change = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    previous_user_token_id = table.Column<long>(type: "bigint", nullable: true),
                    access_token_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    refresh_token_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    access_token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    refresh_token_expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    initial_refresh_token_created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_tokens_user_tokens_previous_user_token_id",
                        column: x => x.previous_user_token_id,
                        principalTable: "user_tokens",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_job",
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
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_queued_job",
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
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hangfire_state",
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
                        principalTable: "hangfire_job",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_counter_expire_at",
                table: "hangfire_counter",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_counter_key_value",
                table: "hangfire_counter",
                columns: new[] { "key", "value" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_hash_expire_at",
                table: "hangfire_hash",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_expire_at",
                table: "hangfire_job",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_state_id",
                table: "hangfire_job",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_job_state_name",
                table: "hangfire_job",
                column: "state_name");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_list_expire_at",
                table: "hangfire_list",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_queued_job_job_id",
                table: "hangfire_queued_job",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_queued_job_queue_fetched_at",
                table: "hangfire_queued_job",
                columns: new[] { "queue", "fetched_at" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_server_heartbeat",
                table: "hangfire_server",
                column: "heartbeat");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_set_expire_at",
                table: "hangfire_set",
                column: "expire_at");

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_set_key_score",
                table: "hangfire_set",
                columns: new[] { "key", "score" });

            migrationBuilder.CreateIndex(
                name: "ix_hangfire_state_job_id",
                table: "hangfire_state",
                column: "job_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_tokens_access_token_hash",
                table: "user_tokens",
                column: "access_token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_tokens_previous_user_token_id",
                table: "user_tokens",
                column: "previous_user_token_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_tokens_refresh_token_hash",
                table: "user_tokens",
                column: "refresh_token_hash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_tokens_user_id",
                table: "user_tokens",
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
                name: "fk_hangfire_job_hangfire_state_state_id",
                table: "hangfire_job",
                column: "state_id",
                principalTable: "hangfire_state",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_hangfire_job_hangfire_state_state_id",
                table: "hangfire_job");

            migrationBuilder.DropTable(
                name: "hangfire_counter");

            migrationBuilder.DropTable(
                name: "hangfire_hash");

            migrationBuilder.DropTable(
                name: "hangfire_job_parameter");

            migrationBuilder.DropTable(
                name: "hangfire_list");

            migrationBuilder.DropTable(
                name: "hangfire_lock");

            migrationBuilder.DropTable(
                name: "hangfire_queued_job");

            migrationBuilder.DropTable(
                name: "hangfire_server");

            migrationBuilder.DropTable(
                name: "hangfire_set");

            migrationBuilder.DropTable(
                name: "user_tokens");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "hangfire_state");

            migrationBuilder.DropTable(
                name: "hangfire_job");
        }
    }
}
