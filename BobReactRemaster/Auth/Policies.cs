﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BobReactRemaster.Auth
{
    public class Policies
    {
        public const string Admin = "Admin";
        public const string User = "User";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Admin).Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            //Because an Admin is also an User
            string[] Roles = { Admin,User};
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser().RequireRole(Roles).Build();
        }
    }
}
