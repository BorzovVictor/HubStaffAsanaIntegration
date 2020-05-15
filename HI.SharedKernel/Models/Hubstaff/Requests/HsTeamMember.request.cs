using System;


namespace HI.SharedKernel.Models
{
    public class HsTeamMemberRequest
    {
        /// <summary>
        /// Start date (yyyy-mm-dd)
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date (yyyy-mm-dd)
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// List of organization IDs (comma separated)
        /// </summary>
        public string Organizations { get; set; }

        /// <summary>
        /// List of project IDs (comma separated)
        /// </summary>
        public string Projects { get; set; }

        /// <summary>
        /// List of users IDs (comma separated)
        /// </summary>
        public string Users { get; set; }

        /// <summary>
        /// Include tasks in report (default: false)
        /// </summary>
        public bool? ShowTasks { get; set; }

        /// <summary>
        /// Include notes in report (default: false)
        /// </summary>
        public bool? ShowNotes { get; set; }

        /// <summary>
        /// Include activity percentages in report (default: false)
        /// </summary>
        public bool? ShowActivity { get; set; }

        /// <summary>
        /// Include archived projects in report (default: false)
        /// </summary>
        public bool? IncludeArchived { get; set; }
    }
}