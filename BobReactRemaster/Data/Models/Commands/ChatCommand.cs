using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data.Models.Stream;

namespace BobReactRemaster.Data.Models.Commands
{
    public abstract class ChatCommand
    {
        [Key] public int ID { get; set; }
        public string Name { get; set; }
        public string Response { get; set; }
        public bool Active { get; set; }

        public int? LiveStreamId { get; set; }

        public LiveStream? LiveStream { get; set; }

    }
}
