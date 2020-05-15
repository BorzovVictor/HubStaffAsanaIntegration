using System.Collections.Generic;
using Newtonsoft.Json;

namespace HI.SharedKernel.Models
{
    public class HubstaffTasksModel
    {
        [JsonProperty("tasks")]
        public List<HubstaffTaskModel> Tasks { get; set; }
    }
}