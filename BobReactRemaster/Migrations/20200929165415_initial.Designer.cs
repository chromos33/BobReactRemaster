﻿// <auto-generated />
using System;
using BobReactRemaster.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BobReactRemaster.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200929165415_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.DiscordCredentials", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClientID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.ToTable("DiscordCredentials");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.Guild", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("Name");

                    b.ToTable("Guild");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.TextChannel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("GuildName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("IsPermanentRelayChannel")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRelayChannel")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("id");

                    b.HasIndex("GuildName");

                    b.ToTable("DiscordTextChannels");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.StreamSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("LiveStreamId")
                        .HasColumnType("int");

                    b.Property<string>("MemberUserName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<bool>("isSubscribed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("LiveStreamId");

                    b.HasIndex("MemberUserName");

                    b.ToTable("StreamSubscriptions");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.Twitch.TwitchCredentials", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClientID")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Secret")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Token")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("isTwitchCheckerClient")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("id");

                    b.ToTable("TwitchCredentials");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.TwitchStream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("RelayChannelid")
                        .HasColumnType("int");

                    b.Property<DateTime>("Started")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("Stopped")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StreamName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("URL")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("RelayChannelid");

                    b.ToTable("TwitchStreams");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.User.Member", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.HasKey("UserName");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.TextChannel", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Discord.Guild", "Guild")
                        .WithMany("TextChannels")
                        .HasForeignKey("GuildName");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.StreamSubscription", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.TwitchStream", "LiveStream")
                        .WithMany("Subscriptions")
                        .HasForeignKey("LiveStreamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Member")
                        .WithMany("StreamSubscriptions")
                        .HasForeignKey("MemberUserName")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.TwitchStream", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Discord.TextChannel", "RelayChannel")
                        .WithMany()
                        .HasForeignKey("RelayChannelid");
                });
#pragma warning restore 612, 618
        }
    }
}