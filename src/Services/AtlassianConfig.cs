namespace Schedule.Importer.Service.Configuration.Abstract
{
    public class AtlassianConfig : IAtlassianConfig
    {
        public string Host { get; set; }

        public string Username { get; set; }

        public string ApiToken { get; set; }
    }
}