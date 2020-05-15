namespace HI.Asana
{
    public class AsanaSettings
    {
        public static AsanaSettings Current;

        public AsanaSettings()
        {
            Current = this;
        }
        public string Token { get; set; }
        public string BaseUrl { get; set; }
    }
}