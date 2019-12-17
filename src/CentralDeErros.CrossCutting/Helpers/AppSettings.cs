namespace CentralDeErros.CrossCutting.Helpers
{
    public class AppSettings
    {
        public string SecretKeyJWT { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Minutes { get; set; }
    }
}
