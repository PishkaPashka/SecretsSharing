using Microsoft.AspNetCore.Http;

namespace SecretsSharing.ViewModels
{
    public class SecretViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsOneUse { get; set; }
        public string Content { get; set; }
    }
}
