﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PandaWebApi.Contexts;

#nullable disable

namespace PandaWebApi.Migrations
{
    [DbContext(typeof(PostgresContext))]
    [Migration("20231216205436_v1")]
    partial class v1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PandaWebApi.Models.Token", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_date");

                    b.Property<byte[]>("SignatureHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("signature_hash");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_tokens");

                    b.HasIndex("ExpirationDate")
                        .HasDatabaseName("ix_tokens_expiration_date");

                    b.HasIndex("SignatureHash")
                        .HasDatabaseName("ix_tokens_signature_hash");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_tokens_user_id");

                    b.ToTable("tokens", (string)null);
                });

            modelBuilder.Entity("PandaWebApi.Models.User", b =>
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

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("FullName")
                        .HasDatabaseName("ix_users_full_name");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasDatabaseName("ix_users_username");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("PandaWebApi.Models.UserAuthenticationHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsAuthenticated")
                        .HasColumnType("boolean")
                        .HasColumnName("is_authenticated");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_authentication_history");

                    b.HasIndex("CreatedAt")
                        .HasDatabaseName("ix_user_authentication_history_created_at");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_authentication_history_user_id");

                    b.ToTable("user_authentication_history", (string)null);
                });

            modelBuilder.Entity("PandaWebApi.Models.Token", b =>
                {
                    b.HasOne("PandaWebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_tokens_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PandaWebApi.Models.UserAuthenticationHistory", b =>
                {
                    b.HasOne("PandaWebApi.Models.User", "User")
                        .WithMany("UserAuthenticationHistories")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_authentication_history_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PandaWebApi.Models.User", b =>
                {
                    b.Navigation("UserAuthenticationHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
