﻿// <auto-generated />
using System;
using Kirel.Logger.HTTP.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kirel.Logger.HTTP.API.Migrations.Mysql
{
    [DbContext(typeof(KirelLogHttpMysqlDbContext))]
    [Migration("20230317103013_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Kirel.Logger.HTTP.API.Models.KirelLogHttp", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36)
                        .HasColumnType("char(36)");

                    b.Property<string>("ClientIp")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Host")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Method")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Path")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Protocol")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("QueryString")
                        .HasColumnType("longtext");

                    b.Property<string>("RequestBody")
                        .HasColumnType("longtext");

                    b.Property<string>("RequestHeaders")
                        .HasColumnType("longtext");

                    b.Property<string>("RequestId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ResponseBody")
                        .HasColumnType("longtext");

                    b.Property<int>("ResponseCode")
                        .HasColumnType("int");

                    b.Property<string>("ResponseHeaders")
                        .HasColumnType("longtext");

                    b.Property<string>("Source")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Username")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Username", "Source", "Host", "Path", "Method", "Protocol", "ClientIp", "RequestId", "ResponseCode");

                    b.ToTable("HttpLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
