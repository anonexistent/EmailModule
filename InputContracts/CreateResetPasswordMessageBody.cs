using System.Text.Json.Serialization;

namespace InputContracts;

public class CreateResetPasswordMessageBody
{
    [JsonPropertyName("to")]
    public string To { get; init; }
}
