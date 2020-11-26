using System;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.Stream;
using BobReactRemaster.EventBus;
using BobReactRemaster.EventBus.Interfaces;
using BobReactRemaster.EventBus.MessageDataTypes;
using IdentityServer4.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;

namespace BobReactRemaster.Services.Stream.Twitch
{
    public class TwitchStreamChecker : IStreamChecker
    {
        private IMessageBus bus { get; set; }
        private IServiceScopeFactory scopefactory { get; set; }
        protected TwitchAPI api;
        private bool Inited { get; set; }
        public TwitchStreamChecker(IMessageBus bus, IServiceScopeFactory scopefactory)
        {
            this.bus = bus;
            this.scopefactory = scopefactory;
            InitTwitchAPI();
        }

        private void InitTwitchAPI()
        {
            api = new TwitchAPI();
            var scope = scopefactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var APICredential = context.TwitchCredentials.FirstOrDefault(x => x.isMainAccount);
            if (APICredential != null)
            {
                api.Settings.AccessToken = APICredential.Token;
                api.Settings.ClientId = APICredential.ClientID;
            }

        }

        private bool inProgress = false;
#pragma warning disable 1998
        public async Task CheckStreams()
#pragma warning restore 1998
        {
            if (!inProgress)
            {
                inProgress = true;
                try
                {
                    if (!Inited)
                    {
                        InitTwitchAPI();
                        Inited = true;
                    }
                    
                    var scope = scopefactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var StreamIDs = context.TwitchStreams.AsQueryable().Where(x => x.StreamID != null && x.StreamID != "").Select(x => x.StreamID).ToList();
                    var requestData = await api.Helix.Streams.GetStreamsAsync(userIds: StreamIDs);
                    HandleStreamStateChange(requestData.Streams);
                }
#pragma warning disable 168
                catch (Exception e)
#pragma warning restore 168
                {
                }

                inProgress = false;
            }
        }

        private void HandleStreamStateChange(TwitchLib.Api.Helix.Models.Streams.Stream[] requestDataStreams)
        {
            var scope = scopefactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var TwitchStreams = context.TwitchStreams.Include(x => x.RelayChannel).AsQueryable().Where(x => x.StreamID != null);
            foreach (TwitchStream stream in TwitchStreams)
            {
                var requestData = requestDataStreams.FirstOrDefault(x => x.UserId == stream.StreamID);
                if (requestData != null)
                {
                    stream.SetStreamStarted(requestData.StartedAt);
                    if (stream.State == StreamState.Stopped)
                    {
                        stream.StartStream();
                        if (stream.RelayEnabled && stream.VariableRelayChannel && stream.RelayChannel == null)
                        {
                            var scope2 = scopefactory.CreateScope();
                            var context2 = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            var freetextchannel = context2.DiscordTextChannels.AsQueryable().FirstOrDefault(x => x.IsPermanentRelayChannel == false && x.LiveStreamID == null);
                            if (freetextchannel != null)
                            {
                                stream.SetRelayChannel(freetextchannel);
                                context2.SaveChanges();
                            }
                        }
                        bus.Publish(new TwitchStreamStartMessageData() { Title = requestData.Title, Streamname = requestData.UserName });
                        
                        
                    }
                    else
                    {
                        if (stream.RelayEnabled && stream.VariableRelayChannel && stream.RelayChannel == null)
                        {
                            var scope2 = scopefactory.CreateScope();
                            var context2 = scope2.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            var freetextchannel = context2.DiscordTextChannels.AsQueryable().FirstOrDefault(x => x.IsPermanentRelayChannel == false && x.LiveStreamID == null);
                            if (freetextchannel != null)
                            {
                                stream.SetRelayChannel(freetextchannel);
                                context2.SaveChanges();
                            }
                        }
                        bus.Publish(new TwitchRelayPulseMessageData(){StreamName = requestData.UserName});
                    }
                    
                }
                else
                {
                    if (stream.State == StreamState.Running)
                    {
                        stream.StopStream();
                        if (stream.RelayEnabled && stream.VariableRelayChannel && stream.RelayChannel != null)
                        {
                            stream.UnsetRelayChannel();
                        }
                        bus.Publish(new TwitchStreamStopMessageData() { StreamName = stream.StreamName });
                    }
                }
            }
            context.SaveChanges();

            
        }
    }
}
