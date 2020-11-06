﻿// <auto-generated />
using System;
using Core.Domain.DbEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Domain.DbEntities.PriorityLevel", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("PriorityLevel");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.Project", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(250)")
                        .HasMaxLength(250);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.Sections", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.Tasks", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AssignedBy")
                        .HasColumnType("int");

                    b.Property<int?>("AssignedFor")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int?>("ParentId")
                        .HasColumnType("int");

                    b.Property<int?>("PriorityId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<bool>("Reminder")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ReminderSchedule")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("Schedule")
                        .HasColumnType("datetime");

                    b.Property<string>("ScheduleString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SectionId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("PriorityId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SectionId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.UserProjects", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasMaxLength(450);

                    b.HasIndex("ProjectId");

                    b.ToTable("UserProjects");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.Sections", b =>
                {
                    b.HasOne("Core.Domain.DbEntities.Project", "Project")
                        .WithMany("Sections")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_Sections_Project");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.Tasks", b =>
                {
                    b.HasOne("Core.Domain.DbEntities.Tasks", "Parent")
                        .WithMany("InverseParent")
                        .HasForeignKey("ParentId")
                        .HasConstraintName("FK_Tasks_Tasks");

                    b.HasOne("Core.Domain.DbEntities.PriorityLevel", "Priority")
                        .WithMany("Tasks")
                        .HasForeignKey("PriorityId")
                        .HasConstraintName("FK_Tasks_PriorityLevel");

                    b.HasOne("Core.Domain.DbEntities.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_Tasks_Project");

                    b.HasOne("Core.Domain.DbEntities.Sections", "Section")
                        .WithMany("Tasks")
                        .HasForeignKey("SectionId")
                        .HasConstraintName("FK_Tasks_Sections");
                });

            modelBuilder.Entity("Core.Domain.DbEntities.UserProjects", b =>
                {
                    b.HasOne("Core.Domain.DbEntities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_UserProjects_Project")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
