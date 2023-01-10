namespace SecretsSharing.Models.Secrets
{
    public class SecretBase
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsOneUse { get; set; }
    }
}