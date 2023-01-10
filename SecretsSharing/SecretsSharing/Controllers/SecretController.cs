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

        private string _userName => User.Identity.Name;

        public SecretController(ISecretObjectService secretObjectService)
        {
            _secretObjectService = secretObjectService;
        }

        [HttpPost]
        [Authorize]
        [Route("/secret/upload")]
        public IActionResult Upload(SecretViewModel secret)
        {
            var secretUrl = _secretObjectService.Add(secret, _userName);
            return Ok(secretUrl);
        }

        [HttpGet]
        [Authorize]
        [Route("/secret/get-all")]
        public IEnumerable<Secret> GetAllSecrets()
        {
            return _secretObjectService.GetAllByUserName(_userName);
        }

        [HttpDelete]
        [Authorize]
        [Route("/secret/delete/{id}")]
        public IActionResult Delete(string id)
        {
            _secretObjectService.Remove(id, _userName);
            return Ok(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("/secret/show/{id}")]
        public string Show(string id)
        {
            var secret = _secretObjectService.GetById(id);
            return secret;
        }
    }
}
