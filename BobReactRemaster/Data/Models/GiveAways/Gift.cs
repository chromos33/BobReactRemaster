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

        public GiveAway GiveAway { get; set; }

        public string Name { get; set; }
        //Name that gets scrapped from Linkpage if it is Steam
        public string InternalName { get; set; }    
        public string Link { get; set; }
        public string Key { get; set; }

        public Member Owner { get; set; }
        public Member? Winner { get; set; } = null;

        public int Turn { get; set; } = 0;
        public bool IsCurrent { get; set; } = false;

        private Gift()
        {

        }
        public Gift(Member Owner,string Link,string Name = "")
        {
            Created = DateTime.Now; 
            this.Owner = Owner;
            this.Name = Name;
            this.Link = Link;    
        }
    }
}
