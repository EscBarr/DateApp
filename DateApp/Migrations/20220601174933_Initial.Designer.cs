﻿// <auto-generated />
using System;
using System.Collections.Generic;
using DateApp.EntityContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DateApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20220601174933_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DateApp.EntityContext.AllServices", b =>
                {
                    b.Property<string>("Services")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.ToTable("ListAllServices");
                });

            modelBuilder.Entity("DateApp.EntityContext.CompletedServices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Usedservices")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("CompletedServices");
                });

            modelBuilder.Entity("DateApp.EntityContext.Couples", b =>
                {
                    b.Property<int>("FirstUserId")
                        .HasColumnType("integer");

                    b.Property<int>("SecondUserId")
                        .HasColumnType("integer");

                    b.HasKey("FirstUserId", "SecondUserId");

                    b.ToTable("User_Couples");
                });

            modelBuilder.Entity("DateApp.EntityContext.EducationDegrees", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Degree")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Educations");
                });

            modelBuilder.Entity("DateApp.EntityContext.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AboutMe")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Adress")
                        .HasColumnType("text");

                    b.Property<byte[]>("Avatar")
                        .HasColumnType("bytea");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("text");

                    b.Property<int?>("EducationId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EyeColor")
                        .HasColumnType("text");

                    b.Property<bool?>("Gender")
                        .HasColumnType("boolean");

                    b.Property<int?>("Height")
                        .HasColumnType("integer");

                    b.Property<bool?>("Is_Participating")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OpinionOnAlcohol")
                        .HasColumnType("text");

                    b.Property<string>("OpinionOnSmoking")
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("PwHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<List<string>>("RecievedLikes")
                        .HasColumnType("text[]");

                    b.Property<string>("ReligionType")
                        .HasColumnType("text");

                    b.Property<List<string>>("SentLikes")
                        .HasColumnType("text[]");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<string>("UserRole")
                        .HasColumnType("text");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EducationId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DateApp.EntityContext.User", b =>
                {
                    b.HasOne("DateApp.EntityContext.EducationDegrees", "Education")
                        .WithMany()
                        .HasForeignKey("EducationId");

                    b.Navigation("Education");
                });
#pragma warning restore 612, 618
        }
    }
}
