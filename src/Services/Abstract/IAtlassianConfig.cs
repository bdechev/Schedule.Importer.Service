namespace Schedule.Importer.Service.Configuration.Abstract
{
    public interface IAtlassianConfig
    {
        string Host { get; set; }

        string Username { get; set; }

        string ApiToken { get; set; }
    }
}
