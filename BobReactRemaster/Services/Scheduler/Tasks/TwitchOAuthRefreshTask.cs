using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.JSONModels.Twitch;
using BobReactRemaster.SettingsOptions;
using IdentityServer4.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BobReactRemaster.Services.Scheduler.Tasks
{
    public class TwitchOAuthRefreshTask: IScheduledTask
    {
        private DateTime NextExecutionDate;
        private IServiceScopeFactory _scopeFactory;
        private int CredentialID;
        private int? ID;
        private readonly WebServerSettingsOptions _options;
        private bool isRemoveable = false;
        public TwitchOAuthRefreshTask(DateTime nextExecutionDate,int credentialID, IServiceScopeFactory scopeFactory = null)
        {
            NextExecutionDate = nextExecutionDate;
            _scopeFactory = scopeFactory;
            CredentialID = credentialID;
        }

        public void setScopeFactory(IServiceScopeFactory factory)
        {
            this._scopeFactory = factory;
        }

        public bool isThisTask(int ID)
        {
            return ID == this.ID;
        }

        public bool InitializeID(int ID)
        {
            if (this.ID == null)
            {
                this.ID = ID;
                return true;
            }

            return false;
        }

        public void QueueRemoval()
        {
            //Can never be Queued
        }

        public bool isRefreshableTask()
        {
            return false;
        }

        public void Update(IScheduledTask task)
        {
            //ignore nothing to update
        }

        public bool isThisTask(IScheduledTask Task)
        {
            return Task == this;
        }

        public int? GetID()
        {
            return CredentialID;
        }

        public bool Executable()
        {
            return DateTime.Compare(NextExecutionDate, DateTime.Now) < 0;
        }
        //Not reasonably Unit Testable as we need to Query Twitch API
        public async void Execute()
        {
            var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //Do not replace with !x.RefreshToken.IsNullOrEmpty() as Queriable can't resolve that automatically anymore
            var credential = context.TwitchCredentials.FirstOrDefault(x => x.id.Equals(CredentialID) && x.RefreshToken != null && x.RefreshToken != "");
            if (credential != null)
            {
                
                var httpclient = new HttpClient();
                var Option = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<WebServerSettingsOptions>>();
                string baseUrl = Option.Value.Address;
                string url = $"https://id.twitch.tv/oauth2/token?grant_type=refresh_token&refresh_token={credential.RefreshToken}&client_id={credential.ClientID}&client_secret={credential.Secret}";
                var response = await httpclient.PostAsync(url, new StringContent("", System.Text.Encoding.UTF8, "text/plain"));
                var responsestring = await response.Content.ReadAsStringAsync();
                TwitchOAuthRefreshData refresh = JsonConvert.DeserializeObject<TwitchOAuthRefreshData>(responsestring);
                if (refresh.error == null)
                {
                    credential.Token = refresh.access_token;
                    credential.RefreshToken = refresh.refresh_token;
                    credential.ExpireDate = DateTime.Now.AddSeconds(refresh.expires_in).Subtract(TimeSpan.FromMinutes(5));
                    context.SaveChangesAsync();
                    NextExecutionDate = credential.ExpireDate;
                }
                
            }
            else
            {
                isRemoveable = true;
            }
            
        }

        public bool Removeable()
        {
            return isRemoveable;
        }
    }
}
