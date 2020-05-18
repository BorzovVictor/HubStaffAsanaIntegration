using System.Collections.Generic;
using Newtonsoft.Json;

namespace HI.SharedKernel.Models.Asana
{
    public class AsanaTaskUpdateModel
    {
        [JsonProperty("data")]
        public DataCustomFields Data { get; set; }
    }
    
    public class DataCustomFields
    {
        [JsonProperty("custom_fields")]
        public Dictionary<string, string> CustomFields { get; set; }
    }
}