namespace dndsitgen.Models
{
    public class GroqMessage
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }

    public class GroqChoice
    {
        public int index { get; set; }
        public GroqMessage? message { get; set; }
    }

    public class GroqResponse
    {
        public List<GroqChoice>? choices { get; set; }
    }
}
