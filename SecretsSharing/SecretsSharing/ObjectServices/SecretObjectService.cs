using AutoMapper;
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
        string Add(SecretViewModel secret, string userName);
        void Remove(string id, string userName);
        IEnumerable<Secret> GetAllByUserName(string userName);
        string GetById(string id);
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

        public string Add(SecretViewModel secret, string userName)
        {
            var entity = _mapper.Map<Secret>(secret);

            var entityId = Guid.NewGuid().ToString();
            entity.Id = entityId;
            entity.UserName = userName;

            _context.Secrets.Add(entity);
            _context.SaveChanges();

            return entityId;
        }

        public IEnumerable<Secret> GetAllByUserName(string userName)
        {
            return _context.Secrets
                .Where(ts => ts.UserName == userName)
                .ToArray();
        }

        public string GetById(string id)
        {
            var secret = _context.Secrets
                .FirstOrDefault(s => s.Id == id);

            if (secret == null) return $"No secrets with id = {id}";

            if (secret.IsOneUse) Remove(secret);

            return secret.Text;
        }

        public void Remove(string id, string userName)
        {
            var secret = _context.Secrets
                .FirstOrDefault(s => s.UserName == userName && s.Id == id);

            if (secret == null) return;

            Remove(secret);
        }

        private void Remove(Secret secret)
        {
            _context.Secrets.Remove(secret);
            _context.SaveChanges();
        }
    }
}
