using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.Models.Secrets;
using SecretsSharing.ObjectServices;
using SecretsSharing.ViewModels;
using System.Collections.Generic;

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
            var userName = User.Identity.Name;
            var secretUrl = _secretObjectService.AddSecret(secret, userName);
            return Ok(secretUrl);
        }

        [HttpGet]
        [Authorize]
        [Route("/secret/get-all")]
        public IEnumerable<Secret> GetAllSecrets()
        {
            var userName = User.Identity.Name;
            return _secretObjectService.GetAllByUserName(userName);
        }
    }
}
