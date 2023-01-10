namespace SecretsSharing.Models.Secrets
{
    public class Secret
    {
        public string Id { get; set; }
        public bool IsOneUse { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
    }
}