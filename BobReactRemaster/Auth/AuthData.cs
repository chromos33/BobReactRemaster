﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Auth
{
    public class AuthData
    {
        [Required] public string UserName { get; set; }       
        [Required] public string Password { get; set; }  
    }
    public class PasswordRequestData
    {
        [Required] public string UserName { get; set; }
    }
}
