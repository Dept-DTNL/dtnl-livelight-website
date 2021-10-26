﻿// <auto-generated />
using System;
using DTNL.LL.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DTNL.LL.DAL.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DTNL.LL.Models.LifxLight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConversionColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ConversionCycle")
                        .HasColumnType("int");

                    b.Property<double>("ConversionPeriod")
                        .HasColumnType("float");

                    b.Property<bool>("GuideEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("HighTrafficAmount")
                        .HasColumnType("int");

                    b.Property<double>("HighTrafficBrightness")
                        .HasColumnType("float");

                    b.Property<string>("HighTrafficColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LifxApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LightGroupName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("LowTrafficBrightness")
                        .HasColumnType("float");

                    b.Property<string>("LowTrafficColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MediumTrafficAmount")
                        .HasColumnType("int");

                    b.Property<double>("MediumTrafficBrightness")
                        .HasColumnType("float");

                    b.Property<string>("MediumTrafficColor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<bool>("TimeRangeEnabled")
                        .HasColumnType("bit");

                    b.Property<TimeSpan>("TimeRangeEnd")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("TimeRangeStart")
                        .HasColumnType("time");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("LifxLights");
                });

            modelBuilder.Entity("DTNL.LL.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<int>("AnalyticsVersion")
                        .HasColumnType("int");

                    b.Property<string>("ConversionTags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("GaProperty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PollingTimeInMinutes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DTNL.LL.Models.LifxLight", b =>
                {
                    b.HasOne("DTNL.LL.Models.Project", "Project")
                        .WithMany("LifxLights")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DTNL.LL.Models.Project", b =>
                {
                    b.Navigation("LifxLights");
                });
#pragma warning restore 612, 618
        }
    }
}
