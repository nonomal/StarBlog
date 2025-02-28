﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StarBlog.Data;

#nullable disable

namespace StarBlog.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231130083039_AlterTableAndFieldsName")]
    partial class AlterTableAndFieldsName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.18");

            modelBuilder.Entity("StarBlog.Data.Models.VisitRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Ip")
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .HasColumnName("ip");

                    b.Property<string>("RequestMethod")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT")
                        .HasColumnName("request_method");

                    b.Property<string>("RequestPath")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT")
                        .HasColumnName("request_path");

                    b.Property<string>("RequestQueryString")
                        .HasMaxLength(2048)
                        .HasColumnType("TEXT")
                        .HasColumnName("request_query_string");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT")
                        .HasColumnName("time");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT")
                        .HasColumnName("user_agent");

                    b.HasKey("Id")
                        .HasName("pk_visit_record");

                    b.ToTable("visit_record", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
