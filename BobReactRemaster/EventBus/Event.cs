namespace BobReactRemaster.EventBus
{
    public class Event<TPayLoad> 
    {
        public TPayLoad PayLoad { get; private set; }

        public Event(TPayLoad payload)
        {
            PayLoad = payload;
        }     
    }
}
