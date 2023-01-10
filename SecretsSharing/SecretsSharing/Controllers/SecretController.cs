using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretsSharing.ObjectServices;
using SecretsSharing.ViewModels;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace SecretsSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecretController : ControllerBase
    {
        private readonly ISecretObjectService _secretObjectService;
        private readonly IWebHostEnvironment _appEnvironment;

        private string _userName => User.Identity.Name;

        public SecretController(ISecretObjectService secretObjectService, IWebHostEnvironment appEnvironment)
        {
            _secretObjectService = secretObjectService;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        [Authorize]
        [Route("/secret/upload")]
        public IActionResult Upload(TextSecretViewModel secret)
        {
            var secretUrl = _secretObjectService.Add(secret, _userName);
            return Ok(secretUrl);
        }

        [HttpPost]
        [Authorize]
        [Route("/secret/upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file, bool isOneUse)
        {
            if (file == null) return Content("File not found");

            var contentRootPath = _appEnvironment.ContentRootPath.Replace('\\', '/');
            string path = $"{contentRootPath}/Files/{file.FileName}";

            using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);

            var secretUrl = _secretObjectService.Add(path, file.FileName, isOneUse, _userName);
            
            return Ok(secretUrl);
        }

        [HttpGet]
        [Authorize]
        [Route("/secret/get-all")]
        public IEnumerable<SecretViewModel> GetAllSecrets()
        {
            return _secretObjectService.GetAllByUserName(_userName);
        }

        [HttpDelete]
        [Authorize]
        [Route("/secret/delete/{id}")]
        public IActionResult Delete(string id)
        {
            _secretObjectService.RemoveTextSecret(id, _userName);
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

        [HttpGet]
        [AllowAnonymous]
        [Route("/secret/download/{id}")]
        public IActionResult Download(string id)
        {
            var secret = _secretObjectService.GetFileById(id);
            var bytes = System.IO.File.ReadAllBytes(secret.Path);

            if (secret.IsOneUse)
            {
                System.IO.File.Delete(secret.Path);
                _secretObjectService.RemoveFileSecret(id, _userName);
            }

            return File(bytes, "application/octet-stream", secret.FileName);
        }
    }
}
