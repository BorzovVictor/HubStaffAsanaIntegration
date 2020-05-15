using System.Collections.Generic;
using Newtonsoft.Json;

namespace HI.SharedKernel.Models
{
    public class HsTasksModel
    {
        [JsonProperty("tasks")]
        public List<HsTaskModel> Tasks { get; set; }
    }
}