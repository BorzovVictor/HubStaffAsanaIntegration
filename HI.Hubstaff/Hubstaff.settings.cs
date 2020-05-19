namespace HI.Hubstaff
{
    public class HubstaffSettings
    {
        public static HubstaffSettings Current;

        public HubstaffSettings()
        {
            Current = this;
        }
        public string AppToken { get; set; }
        public string AuthToken { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string BaseUrl { get; set; }
        public int HoursBetweenCheck { get; set; }
    }
}