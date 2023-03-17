﻿// <auto-generated />
using System;
using Kirel.Logger.HTTP.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kirel.Logger.HTTP.API.Migrations.Mssql
{
    [DbContext(typeof(KirelLogHttpMssqlDbContext))]
    partial class KirelLogHttpMssqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Kirel.Logger.HTTP.API.Models.KirelLogHttp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientIp")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Method")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Protocol")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("QueryString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ResponseCode")
                        .HasColumnType("int");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username", "Source", "Host", "Path", "Method", "Protocol", "ClientIp", "RequestId", "ResponseCode");

                    b.ToTable("HttpLogs");
                });
#pragma warning restore 612, 618
        }
    }
}