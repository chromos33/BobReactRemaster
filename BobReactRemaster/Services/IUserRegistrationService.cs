using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BobReactRemaster.Services
{
    interface IUserRegistrationService
    {
        public string RegisterUser(string nickname, ulong discordid, bool subscribe = true);
    }
}
