namespace SecretsSharing.Models.Secrets
{
    public class FileSecret : SecretBase
    {
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
