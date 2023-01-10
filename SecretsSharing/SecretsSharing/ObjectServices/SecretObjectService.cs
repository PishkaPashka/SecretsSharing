using AutoMapper;
using AutoMapper.QueryableExtensions;
using SecretsSharing.Models;
using SecretsSharing.Models.Secrets;
using SecretsSharing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretsSharing.ObjectServices
{
    public interface ISecretObjectService
    {
        string Add(TextSecretViewModel secret, string userName);
        string Add(string path, string fileName, bool isOneUse, string userName);
        void Remove(string id, string userName);
        IEnumerable<SecretViewModel> GetAllByUserName(string userName);
        string GetById(string id);
        FileSecret GetFileById(string id);
    }

    public class SecretObjectService : ISecretObjectService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;

        public SecretObjectService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public string Add(TextSecretViewModel secret, string userName)
        {
            var entity = _mapper.Map<TextSecret>(secret);

            var entityId = Guid.NewGuid().ToString();
            entity.Id = entityId;
            entity.UserName = userName;

            _context.TextSecrets.Add(entity);
            _context.SaveChanges();

            return entityId;
        }

        public string Add(string path, string fileName, bool isOneUse, string userName)
        {
            var entityId = Guid.NewGuid().ToString();
            var entity = new FileSecret { Id = entityId, UserName = userName, Path = path, FileName = fileName, IsOneUse = isOneUse };

            _context.FileSecrets.Add(entity);
            _context.SaveChanges();

            return entityId;
        }

        public FileSecret GetFileById(string id)
        {
            return _context.FileSecrets
                .FirstOrDefault(f => f.Id == id);
        }

        public IEnumerable<SecretViewModel> GetAllByUserName(string userName)
        {
            var files = _context.FileSecrets
                .Where(ts => ts.UserName == userName)
                .ProjectTo<SecretViewModel>(_mapper.ConfigurationProvider);

            var texts = _context.TextSecrets
                .Where(ts => ts.UserName == userName)
                .ProjectTo<SecretViewModel>(_mapper.ConfigurationProvider);

            var secrets = texts.Concat(files)
                .ToArray();

            return secrets;
        }

        public string GetById(string id)
        {
            var secret = _context.TextSecrets
                .FirstOrDefault(s => s.Id == id);

            if (secret == null) return $"No secrets with id = {id}";

            if (secret.IsOneUse) Remove(secret);

            return secret.Text;
        }

        public void Remove(string id, string userName)
        {
            var secret = _context.TextSecrets
                .FirstOrDefault(s => s.UserName == userName && s.Id == id);

            if (secret == null) return;

            Remove(secret);
        }

        private void Remove(TextSecret secret)
        {
            _context.TextSecrets.Remove(secret);
            _context.SaveChanges();
        }
    }
}
