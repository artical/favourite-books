using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFavouriteBooks.Models.ViewModels
{
    public class LogEntry
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
        [JsonProperty("logType")]
        public string LogType { get; set; }
        [JsonProperty("details")]
        public string Details { get; set; }
    }
}
