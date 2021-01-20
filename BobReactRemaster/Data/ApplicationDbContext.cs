using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Commands;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Meetings;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.Data.Models.Stream.DLive;
using BobReactRemaster.Data.Models.Stream.Twitch;
using BobReactRemaster.Data.Models.User;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BobReactRemaster.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TwitchStream> TwitchStreams { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<StreamSubscription> StreamSubscriptions { get; set; }
        public DbSet<TextChannel> DiscordTextChannels { get; set; }

        public DbSet<DiscordCredentials> DiscordCredentials { get; set; }

        public DbSet<TwitchCredential> TwitchCredentials { get; set; }

        public DbSet<ManualCommand> ManualCommands { get; set; }

        public DbSet<IntervalCommand> IntervalCommands { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<MeetingTemplate> MeetingTemplates { get; set; }
        public DbSet<MeetingDateTemplate> MeetingDateTemplates { get; set; }
        public DbSet<ReminderTemplate> ReminderTemplates { get; set; }
        public DbSet<MeetingTemplate_Member> MeetingTemplates_Members { get; set; }
        public DbSet<MeetingSubscription> MeetingSubscriptions { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Member>()
                .HasMany(u => u.StreamSubscriptions)
                .WithOne(ss => ss.Member)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Member>()
                .HasMany(m => m.RegisteredToMeetingTemplates)
                .WithOne(t => t.RegisteredMember)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Member>()
                .HasMany(m => m.MeetingSubscriptions)
                .WithOne(s => s.Subscriber)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MeetingTemplate>()
                .HasMany(t => t.Dates)
                .WithOne(d => d.Template)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MeetingTemplate>()
                .HasOne(t => t.ReminderTemplate)
                .WithOne(rt => rt.MeetingTemplate)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Meeting>()
                .HasMany(m => m.Subscriber)
                .WithOne(s => s.Meeting)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<TwitchStream>()
                .HasMany(s => s.Subscriptions)
                .WithOne(ss => (TwitchStream) ss.LiveStream)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TwitchStream>()
                .HasOne(s => s.APICredential)
                .WithOne(c => c.Stream)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
