namespace web.Model
{
    public class Message
    {
        public int Id { get; set; }

        public string Data { get; set; }

        public string DataContentType { get; set; }

        public string CloudEventType { get; set; }

        public string CloudEventId { get; set; }

        public bool IsComplete { get; set; }

    }
}