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
    public class ApplicationDbContext : ApiAuthorizationDbContext<Member>
    {
        DbSet<TwitchStream> TwitchStreams { get; set; }
        DbSet<Member> Members { get; set; }
        DbSet<StreamSubscription> StreamSubscriptions { get; set; }
        DbSet<TextChannel> DiscordTextChannels { get; set; }

        DbSet<DiscordCredentials> DiscordCredentials { get; set; }

        DbSet<TwitchCredentials> TwitchCredentials { get; set; }
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
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
