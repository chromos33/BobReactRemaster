namespace BobReactRemaster.JSONModels.Meeting
{
    public class MeetingGeneralData
    {
        public MeetingGeneralMember[] Members { get; set; }
        public int MeetingID { get; set; }
        public string? Name { get; set; }
    }
    public class MeetingGeneralMember
    {
        public string userName { get; set; }
        public bool registered { get; set; }
    }
}