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
        IEnumerable<SecretViewModel> GetAllByUserName(string userName);
        string GetById(string id);
        FileSecret GetFileById(string id);
        bool DeleteById(string id, string userName);
        void Delete<TEntity>(TEntity entity) where TEntity : class;
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

            Add(entity);

            return entityId;
        }

        public string Add(string path, string fileName, bool isOneUse, string userName)
        {
            var entityId = Guid.NewGuid().ToString();
            var entity = new FileSecret { Id = entityId, UserName = userName, Path = path, FileName = fileName, IsOneUse = isOneUse };

            Add(entity);

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

            if (secret.IsOneUse) Delete(secret);

            return secret.Text;
        }

        public bool DeleteById(string id, string userName)
        {
            var textSecret = _context.TextSecrets
                .FirstOrDefault(s => s.UserName == userName && s.Id == id);

            if (textSecret != null)
            {
                Delete(textSecret);
                return true;
            }

            var fileSecret = _context.FileSecrets
                .FirstOrDefault(s => s.UserName == userName && s.Id == id);

            if (fileSecret != null)
            {
                Delete(fileSecret);
                return true;
            }

            return false;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        private void Add<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Add(entity);
            _context.SaveChanges();
        }
    }
}
