﻿// <auto-generated />
using Base5.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Base5.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210529015146_Initialize_DataBase")]
    partial class Initialize_DataBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Base5.Domain.Entities.KPI", b =>
                {
                    b.Property<int>("KPIDNum")
                        .HasColumnType("int");

                    b.Property<int>("DepNo")
                        .HasColumnType("int");

                    b.Property<string>("KPIDDescription")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("MeasurmentUnit")
                        .HasColumnType("bit");

                    b.Property<int>("TargetValue")
                        .HasColumnType("int");

                    b.HasKey("KPIDNum");

                    b.ToTable("TblKPI");
                });
#pragma warning restore 612, 618
        }
    }
}
