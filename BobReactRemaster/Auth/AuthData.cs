using System;
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

    public class PasswordChangeData
    {
        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }
        [Required] public string NewPasswordRepeat { get; set; }

        public bool NewPasswordsMatch()
        {
            return NewPassword == NewPasswordRepeat;
        }
    }
}
