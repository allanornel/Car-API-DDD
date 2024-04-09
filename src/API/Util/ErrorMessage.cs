using System.Text.Json.Serialization;

namespace API.Util
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;
        }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
