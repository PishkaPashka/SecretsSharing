using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.ObjectServices;
using SecretsSharing.ViewModels;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretController : ControllerBase
    {
        private readonly ISecretObjectService _secretObjectService;

        public SecretController(ISecretObjectService secretObjectService)
        {
            _secretObjectService = secretObjectService;
        }

        [HttpPost]
        [Authorize]
        [Route("/secret/upload")]
        public IActionResult Upload(SecretViewModel secret)
        {
            var secretUrl = _secretObjectService.AddSecret(secret);
            return Ok(secretUrl);
        }
    }
}
