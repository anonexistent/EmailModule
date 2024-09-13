using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OutputContracts;

public class OutputCacheMessage
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("timeStamp")]
    public string TimeStamp { get; set; }

    public OutputCacheMessage(CacheMessage m)
    {
        Id = m.Id.ToString(); Message=m.Message; TimeStamp = m.TimeStamp.ToString();
    }
}

