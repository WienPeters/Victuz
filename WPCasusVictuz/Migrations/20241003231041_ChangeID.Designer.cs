﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WPCasusVictuz.Data;

#nullable disable

namespace WPCasusVictuz.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20241003231041_ChangeID")]
    partial class ChangeID
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("WPCasusVictuz.Models.Aktivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MaxParticipants")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Members");

                    b.HasDiscriminator().HasValue("Member");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Poll", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedByBoardMemberId")
                        .HasColumnType("int");

                    b.Property<string>("Options")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedByBoardMemberId");

                    b.ToTable("Polls");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Registration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AktivityId")
                        .HasColumnType("int");

                    b.Property<int?>("MemberId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AktivityId");

                    b.HasIndex("MemberId");

                    b.ToTable("Registrations");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Vote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("MemberId")
                        .HasColumnType("int");

                    b.Property<int?>("PollId")
                        .HasColumnType("int");

                    b.Property<string>("SelectedOption")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("PollId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.BoardMember", b =>
                {
                    b.HasBaseType("WPCasusVictuz.Models.Member");

                    b.Property<int?>("BoardMemberId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("BoardMember");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Poll", b =>
                {
                    b.HasOne("WPCasusVictuz.Models.BoardMember", "CreatedBy")
                        .WithMany("CreatedPolls")
                        .HasForeignKey("CreatedByBoardMemberId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Registration", b =>
                {
                    b.HasOne("WPCasusVictuz.Models.Aktivity", "Aktivity")
                        .WithMany("Registrations")
                        .HasForeignKey("AktivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WPCasusVictuz.Models.Member", "Member")
                        .WithMany("Registrations")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Aktivity");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Vote", b =>
                {
                    b.HasOne("WPCasusVictuz.Models.Member", "Member")
                        .WithMany("Votes")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WPCasusVictuz.Models.Poll", "Poll")
                        .WithMany("Votes")
                        .HasForeignKey("PollId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Member");

                    b.Navigation("Poll");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Aktivity", b =>
                {
                    b.Navigation("Registrations");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Member", b =>
                {
                    b.Navigation("Registrations");

                    b.Navigation("Votes");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.Poll", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("WPCasusVictuz.Models.BoardMember", b =>
                {
                    b.Navigation("CreatedPolls");
                });
#pragma warning restore 612, 618
        }
    }
}
