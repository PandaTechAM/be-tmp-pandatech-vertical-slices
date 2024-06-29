﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Pandatech.VerticalSlices.Context;

#nullable disable

namespace Pandatech.VerticalSlices.Context.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20240629161832_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MassTransit.PostgresOutbox.Entities.InboxMessage", b =>
                {
                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("message_id");

                    b.Property<string>("ConsumerId")
                        .HasColumnType("text")
                        .HasColumnName("consumer_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("MessageId", "ConsumerId")
                        .HasName("pk_inbox_messages");

                    b.ToTable("inbox_messages", (string)null);
                });

            modelBuilder.Entity("MassTransit.PostgresOutbox.Entities.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("payload");

                    b.Property<int>("State")
                        .HasColumnType("integer")
                        .HasColumnName("state");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_outbox_messages");

                    b.ToTable("outbox_messages", (string)null);
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.Token", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("AccessTokenExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("access_token_expires_at");

                    b.Property<byte[]>("AccessTokenHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("access_token_hash");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("InitialRefreshTokenCreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("initial_refresh_token_created_at");

                    b.Property<long?>("PreviousTokenId")
                        .HasColumnType("bigint")
                        .HasColumnName("previous_token_id");

                    b.Property<DateTime>("RefreshTokenExpiresAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expires_at");

                    b.Property<byte[]>("RefreshTokenHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("refresh_token_hash");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_tokens");

                    b.HasIndex("AccessTokenHash")
                        .IsUnique()
                        .HasDatabaseName("ix_tokens_access_token_hash");

                    b.HasIndex("PreviousTokenId")
                        .IsUnique()
                        .HasDatabaseName("ix_tokens_previous_token_id");

                    b.HasIndex("RefreshTokenHash")
                        .IsUnique()
                        .HasDatabaseName("ix_tokens_refresh_token_hash");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_tokens_user_id");

                    b.ToTable("tokens", (string)null);
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_user_id");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.Property<bool>("ForcePasswordChange")
                        .HasColumnType("boolean")
                        .HasColumnName("force_password_change");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("full_name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<long?>("UpdatedByUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_user_id");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("FullName")
                        .HasDatabaseName("ix_users_full_name");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.UserConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<long?>("CreatedByUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by_user_id");

                    b.Property<bool>("Deleted")
                        .HasColumnType("boolean")
                        .HasColumnName("deleted");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<long?>("UpdatedByUserId")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by_user_id");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.Property<int>("Version")
                        .IsConcurrencyToken()
                        .HasColumnType("integer")
                        .HasColumnName("version");

                    b.HasKey("Id")
                        .HasName("pk_user_configs");

                    b.HasIndex("UserId", "Key")
                        .IsUnique()
                        .HasDatabaseName("ix_user_configs_user_id_key");

                    b.ToTable("user_configs", (string)null);
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.Token", b =>
                {
                    b.HasOne("Pandatech.VerticalSlices.Domain.Entities.Token", "PreviousToken")
                        .WithOne()
                        .HasForeignKey("Pandatech.VerticalSlices.Domain.Entities.Token", "PreviousTokenId")
                        .HasConstraintName("fk_tokens_tokens_previous_token_id");

                    b.HasOne("Pandatech.VerticalSlices.Domain.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tokens_users_user_id");

                    b.Navigation("PreviousToken");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.UserConfig", b =>
                {
                    b.HasOne("Pandatech.VerticalSlices.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_configs_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Pandatech.VerticalSlices.Domain.Entities.User", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}