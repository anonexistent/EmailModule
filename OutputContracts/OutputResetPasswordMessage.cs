using Models;
using System.Text.Json.Serialization;

namespace OutputContracts;

public class OutputResetPasswordMessage
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("to")]
    public string To { get; set; }
    [JsonPropertyName("date")]
    public string Date { get; set; }

    public OutputResetPasswordMessage(ResetPasswordMessage msg)
    {
        Id = msg.Id.ToString();
        To = msg.To;
        Date = msg.Date.ToString();
    }
}
