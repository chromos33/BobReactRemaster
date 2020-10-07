using System;
using System.Collections.Generic;
using System.Text;
using BobReactRemaster.Data.Models.Discord;
using BobReactRemaster.Data.Models.Stream;
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
            builder.Entity<TwitchStream>()
                .HasMany(s => s.Subscriptions)
                .WithOne(ss => (TwitchStream) ss.LiveStream)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
