﻿// <auto-generated />
using AngularMarketplace.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AngularMarketplace.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240915220804_Add price to product")]
    partial class Addpricetoproduct
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("DataAccess.Entities.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("description");

                    b.Property<float>("Price")
                        .HasColumnType("REAL")
                        .HasColumnName("price");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarhcar(50)")
                        .HasColumnName("title");

                    b.Property<string>("img1")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("img2")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("img3")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("img4")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("img5")
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("img6")
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("ID");

                    b.ToTable("tblProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
