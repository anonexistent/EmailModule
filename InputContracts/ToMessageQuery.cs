using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InputContracts;

public class ToMessageQuery
{
    [JsonPropertyName("to")]
    public string? To { get; set; }
    [Required]
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}
