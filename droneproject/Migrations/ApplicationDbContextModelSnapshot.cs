﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using droneproject.DataAccess;

namespace droneproject.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("droneproject.Domain.Drone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Battery")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Createdby")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<double>("LoadingWeight")
                        .HasColumnType("float");

                    b.Property<int>("Model")
                        .HasColumnType("int");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ReferenceKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Drones");
                });

            modelBuilder.Entity("droneproject.Domain.Mediation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Createdby")
                        .HasColumnType("int");

                    b.Property<int>("DroneId")
                        .HasColumnType("int");

                    b.Property<string>("ImageId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("DroneId");

                    b.ToTable("Mediations");
                });

            modelBuilder.Entity("droneproject.Domain.Mediation", b =>
                {
                    b.HasOne("droneproject.Domain.Drone", "Drone")
                        .WithMany()
                        .HasForeignKey("DroneId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Drone");
                });
#pragma warning restore 612, 618
        }
    }
}
