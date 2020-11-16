using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Commands
{
    public abstract class ChatCommand
    {
        [Key] public string ID { get; set; }
        public string Response { get; set; }
        public bool Active { get; set; }

        public int LiveStream { get; set; }

    }
}
