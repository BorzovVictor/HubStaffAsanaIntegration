using System;

namespace HI.SharedKernel.Models.Responses
{
    public class UpdateSumFieldsResult
    {
        public string AsanaTaskId { get; set; }
        public decimal? SumHoursPreviousValue { get; set; }
        public decimal? HubstaffSumHoursValue { get; set; }
        public decimal? SumHoursNewValue { get; set; }
        public string ResultSum => SumHoursNewValue.Value.ToString("F1");
        public DateTime UpdateAt { get; set; }
    }
}