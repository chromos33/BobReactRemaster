using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BobReactRemaster.Data;
using BobReactRemaster.Data.Models.User;
using Microsoft.EntityFrameworkCore;

namespace BobReactRemaster.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Member> CompleteMeetingTemplateFromMember(this ApplicationDbContext context)
        {
            return context.Members
                .Include(x => x.RegisteredToMeetingTemplates).ThenInclude(x => x.MeetingTemplate)
                .Include( x => x.RegisteredToMeetingTemplates).ThenInclude(y => y.MeetingTemplate)
                ;
        }
        public static IQueryable<Member> CompleteMeetingsFromMember(this ApplicationDbContext context)
        {
            return context.Members
                .Include(x => x.MeetingSubscriptions).ThenInclude(x => x.Meeting)
                ;
        }
    }
}
