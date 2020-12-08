using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobReactRemaster.Data.Models.Stream
{
    public class Quote
    {
        [Key] public int Id { get; set; }

        public int LiveStreamID { get; set; }
        [Required] public LiveStream stream { get; set; }
        [Required] public DateTime Created { get; set; }
        [Required] public string Text { get; set; }

        public override string ToString()
        {
            return $"\"{Text}\" - {stream.StreamName}, {Created:MMMM yyyy} (ID {Id})";
        }
    }
}
