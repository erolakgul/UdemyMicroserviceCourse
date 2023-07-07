namespace FreeCourse.Mvc.Web.Models.Configurations
{
    public class ClientSettings
    {
        public Client? WebClientCredential { get; set; }
        public Client? WebResourceOwner { get; set; }
    }

    public class Client
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
