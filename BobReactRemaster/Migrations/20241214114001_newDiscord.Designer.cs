﻿// <auto-generated />
using System;
using BobReactRemaster.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BobReactRemaster.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241214114001_newDiscord")]
    partial class newDiscord
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BobReactRemaster.Data.Models.Commands.IntervalCommand", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("AutoInverval")
                        .HasColumnType("int");

                    b.Property<int?>("LiveStreamId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("LiveStreamId");

                    b.ToTable("IntervalCommands");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Commands.ManualCommand", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("LiveStreamId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Trigger")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.HasIndex("LiveStreamId");

                    b.ToTable("ManualCommands");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.DiscordCredentials", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("ClientID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("DiscordCredentials");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.TextChannel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<ulong>("ChannelID")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Guild")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsPermanentRelayChannel")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsRelayChannel")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("LiveStreamID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.HasIndex("LiveStreamID")
                        .IsUnique();

                    b.ToTable("DiscordTextChannels");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.Gift", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("GiveAwayID")
                        .HasColumnType("int");

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsCurrent")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OwnerUserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Turn")
                        .HasColumnType("int");

                    b.Property<string>("WinnerUserName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("GiveAwayID");

                    b.HasIndex("OwnerUserName");

                    b.HasIndex("WinnerUserName");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.GiveAway", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("TextChannelid")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TextChannelid");

                    b.ToTable("GiveAways");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.GiveAway_Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("GiveAwayID")
                        .HasColumnType("int");

                    b.Property<string>("MemberUserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("GiveAwayID");

                    b.HasIndex("MemberUserName");

                    b.ToTable("GiveAway_Member");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.Meeting", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("IsSingleEvent")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("MeetingDateEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("MeetingDateStart")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MeetingTemplateID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReminderDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("MeetingTemplateID");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingDateTemplate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("TemplateID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TemplateID");

                    b.ToTable("MeetingDateTemplates");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingParticipation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("IsAuthor")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MeetingID")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("longtext");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<string>("SubscriberUserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("MeetingID");

                    b.HasIndex("SubscriberUserName");

                    b.ToTable("MeetingSubscriptions");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("ID");

                    b.ToTable("MeetingTemplates");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingTemplate_Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<bool>("IsAuthor")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MeetingTemplateID")
                        .HasColumnType("int");

                    b.Property<string>("RegisteredMemberUserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("MeetingTemplateID");

                    b.HasIndex("RegisteredMemberUserName");

                    b.ToTable("MeetingTemplates_Members");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.ReminderTemplate", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("MeetingTemplateId")
                        .HasColumnType("int");

                    b.Property<int>("ReminderDay")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReminderTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ID");

                    b.HasIndex("MeetingTemplateId")
                        .IsUnique();

                    b.ToTable("ReminderTemplates");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.LiveStream", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<bool>("RelayEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("Started")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<DateTime>("Stopped")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StreamName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("URL")
                        .HasColumnType("longtext");

                    b.Property<int>("UpTimeInterval")
                        .HasColumnType("int");

                    b.Property<bool>("VariableRelayChannel")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("LiveStream");

                    b.HasDiscriminator().HasValue("LiveStream");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LiveStreamID")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("LiveStreamID");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.StreamSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LiveStreamId")
                        .HasColumnType("int");

                    b.Property<string>("MemberUserName")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("isSubscribed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("LiveStreamId");

                    b.HasIndex("MemberUserName");

                    b.ToTable("StreamSubscriptions");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.Twitch.TwitchCredential", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("ChatUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("ClientID")
                        .HasColumnType("longtext");

                    b.Property<string>("Code")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("ExpireDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<string>("Secret")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.Property<bool>("isMainAccount")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("validationKey")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("TwitchCredentials");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.User.Member", b =>
                {
                    b.Property<string>("UserName")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DiscordDiscriminator")
                        .HasColumnType("longtext");

                    b.Property<ulong?>("DiscordID")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("DiscordUserName")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<int>("TestWonGiftsCount")
                        .HasColumnType("int");

                    b.Property<string>("UserRole")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserName");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.TwitchStream", b =>
                {
                    b.HasBaseType("BobReactRemaster.Data.Models.Stream.LiveStream");

                    b.Property<int?>("APICredentialId")
                        .HasColumnType("int");

                    b.Property<string>("StreamID")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasIndex("APICredentialId")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("TwitchStream");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Commands.IntervalCommand", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.LiveStream", "LiveStream")
                        .WithMany("RelayIntervalCommands")
                        .HasForeignKey("LiveStreamId");

                    b.Navigation("LiveStream");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Commands.ManualCommand", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.LiveStream", "LiveStream")
                        .WithMany("RelayManualCommands")
                        .HasForeignKey("LiveStreamId");

                    b.Navigation("LiveStream");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.TextChannel", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.LiveStream", null)
                        .WithOne("RelayChannel")
                        .HasForeignKey("BobReactRemaster.Data.Models.Discord.TextChannel", "LiveStreamID");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.Gift", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.GiveAways.GiveAway", "GiveAway")
                        .WithMany("Gifts")
                        .HasForeignKey("GiveAwayID");

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Owner")
                        .WithMany("GivenGifts")
                        .HasForeignKey("OwnerUserName")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Winner")
                        .WithMany("WonGifts")
                        .HasForeignKey("WinnerUserName")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("GiveAway");

                    b.Navigation("Owner");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.GiveAway", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Discord.TextChannel", "TextChannel")
                        .WithMany("GiveAways")
                        .HasForeignKey("TextChannelid")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("TextChannel");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.GiveAway_Member", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.GiveAways.GiveAway", "GiveAway")
                        .WithMany("Admins")
                        .HasForeignKey("GiveAwayID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Member")
                        .WithMany("GiveAways")
                        .HasForeignKey("MemberUserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GiveAway");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.Meeting", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", "MeetingTemplate")
                        .WithMany("LiveMeetings")
                        .HasForeignKey("MeetingTemplateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeetingTemplate");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingDateTemplate", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", "Template")
                        .WithMany("Dates")
                        .HasForeignKey("TemplateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingParticipation", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Meetings.Meeting", "Meeting")
                        .WithMany("Subscriber")
                        .HasForeignKey("MeetingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Subscriber")
                        .WithMany("MeetingSubscriptions")
                        .HasForeignKey("SubscriberUserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingTemplate_Member", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", "MeetingTemplate")
                        .WithMany("Members")
                        .HasForeignKey("MeetingTemplateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "RegisteredMember")
                        .WithMany("RegisteredToMeetingTemplates")
                        .HasForeignKey("RegisteredMemberUserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeetingTemplate");

                    b.Navigation("RegisteredMember");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.ReminderTemplate", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", "MeetingTemplate")
                        .WithOne("ReminderTemplate")
                        .HasForeignKey("BobReactRemaster.Data.Models.Meetings.ReminderTemplate", "MeetingTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeetingTemplate");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.Quote", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.LiveStream", "stream")
                        .WithMany("Quotes")
                        .HasForeignKey("LiveStreamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("stream");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.StreamSubscription", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.LiveStream", "LiveStream")
                        .WithMany("Subscriptions")
                        .HasForeignKey("LiveStreamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BobReactRemaster.Data.Models.User.Member", "Member")
                        .WithMany("StreamSubscriptions")
                        .HasForeignKey("MemberUserName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LiveStream");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.TwitchStream", b =>
                {
                    b.HasOne("BobReactRemaster.Data.Models.Stream.Twitch.TwitchCredential", "APICredential")
                        .WithOne("Stream")
                        .HasForeignKey("BobReactRemaster.Data.Models.Stream.TwitchStream", "APICredentialId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("APICredential");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Discord.TextChannel", b =>
                {
                    b.Navigation("GiveAways");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.GiveAways.GiveAway", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Gifts");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.Meeting", b =>
                {
                    b.Navigation("Subscriber");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Meetings.MeetingTemplate", b =>
                {
                    b.Navigation("Dates");

                    b.Navigation("LiveMeetings");

                    b.Navigation("Members");

                    b.Navigation("ReminderTemplate")
                        .IsRequired();
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.LiveStream", b =>
                {
                    b.Navigation("Quotes");

                    b.Navigation("RelayChannel");

                    b.Navigation("RelayIntervalCommands");

                    b.Navigation("RelayManualCommands");

                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.Stream.Twitch.TwitchCredential", b =>
                {
                    b.Navigation("Stream")
                        .IsRequired();
                });

            modelBuilder.Entity("BobReactRemaster.Data.Models.User.Member", b =>
                {
                    b.Navigation("GiveAways");

                    b.Navigation("GivenGifts");

                    b.Navigation("MeetingSubscriptions");

                    b.Navigation("RegisteredToMeetingTemplates");

                    b.Navigation("StreamSubscriptions");

                    b.Navigation("WonGifts");
                });
#pragma warning restore 612, 618
        }
    }
}
