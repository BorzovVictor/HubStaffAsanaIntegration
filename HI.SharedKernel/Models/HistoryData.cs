using System;

namespace HI.SharedKernel.Models
{
    public class HistoryData
    {
        public HistoryData()
        {
            Id = Guid.NewGuid().ToString("N");
            LastUpdate = DateTime.UtcNow;
        }
        public string Id { get; set; }
        public long? HubId { get; set; }
        public string Summary { get; set; }
        public string RemoteId { get; set; }
        public long? Duration { get; set; }

        public string AsanaId { get; set; }
        public string Name { get; set; }
        public string AssigneeStatus { get; set; }
        public bool? Completed { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}