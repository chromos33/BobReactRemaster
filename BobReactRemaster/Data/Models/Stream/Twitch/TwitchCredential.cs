﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Controllers;

namespace BobReactRemaster.Data.Models.Stream.Twitch
{
    public class TwitchCredential
    {
        [Key] public int id { get; set; }
        public string ClientID { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public string Secret { get; set; }
        public string validationKey { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }

        //May only be Changed in Setup/Admin
        public bool isMainAccount { get; set; }

        public void setFromTwitchOauthStoreData(TwitchOauthStoreData data)
        {
            ClientID = data.ClientID;
            Secret = data.Secret;
        }

        public string getTwitchReturnURL(string webserverAddress)
        {
            string returnurl = webserverAddress;
            if (returnurl.Substring(returnurl.Length - 1) != "/")
            {
                returnurl += "/";
            }

            returnurl += "Twitch/TwitchOAuthReturn";
            return returnurl;
        }

        public string getTwitchAuthLink(TwitchOauthStoreData data, string webserverAddress)
        {
            string state = Guid.NewGuid().ToString();
            validationKey = state;
            string returnurl = getTwitchReturnURL(webserverAddress);
            string link = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={ClientID}&redirect_uri={returnurl}";
            if (!string.IsNullOrEmpty(data.Scopes))
            {
                link += "&scope=";
                foreach (string scope in data.Scopes.Split('|'))
                {
                    if (!string.IsNullOrEmpty(scope))
                    {
                        link += scope + "+";
                    }
                }
                link = link.Remove(link.Length - 1);
            }

            link += $"&state={state}";
            return link;
        }
    }
}