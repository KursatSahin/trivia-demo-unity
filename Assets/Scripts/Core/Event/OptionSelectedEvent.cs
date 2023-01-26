namespace Core.Event
{
    public class OptionSelectedEvent : IEvent
    {
        public string OptionId { get; set; }
        
        public OptionSelectedEvent(string optionId)
        {
            OptionId = optionId;
        }
    }
}