﻿// <auto-generated />
using System;
using HI.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HI.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200519135244_YersatdayDurations")]
    partial class YersatdayDurations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4");

            modelBuilder.Entity("HI.SharedKernel.Models.HistoryData", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AsanaId")
                        .HasColumnType("TEXT");

                    b.Property<string>("AssigneeStatus")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Completed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("CompletedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("HubId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastUpdate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("RemoteId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT");

                    b.Property<long>("YesterdayDuration")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Histories");
                });
#pragma warning restore 612, 618
        }
    }
}
