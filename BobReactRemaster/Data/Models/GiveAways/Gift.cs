using BobReactRemaster.Data.Models.User;
using System;
using System.ComponentModel.DataAnnotations;

namespace BobReactRemaster.Data.Models.GiveAways
{
    public class Gift
    {


        [Key]
        public int ID { get; set; }
        
        public DateTime Created { get; }

        public GiveAway GiveAway { get; }

        public string Name { get; set; }
        public string Link { get; set; }
        public string Key { get; set; }

        public Member Owner { get; set; }
        public Member? Winner { get; set; } = null;

        public int Turn { get; set; } = 0;

        private Gift()
        {

        }
        public Gift(GiveAway GiveAway,Member Owner)
        {
            Created = DateTime.Now; 
            this.GiveAway = GiveAway;
            this.Owner = Owner;
        }
    }
}
