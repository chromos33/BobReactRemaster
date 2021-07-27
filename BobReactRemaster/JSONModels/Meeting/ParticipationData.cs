using BobReactRemaster.Data.Models.Meetings;

namespace BobReactRemaster.JSONModels.Meeting
{
    public class ParticipationData
    {
        public int ParticipationID { get; set; }
        public MeetingParticipationState State { get; set; }
        
        public string Info { get; set; }
    }
}