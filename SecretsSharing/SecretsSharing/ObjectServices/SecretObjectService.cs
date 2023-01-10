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
        string AddSecret(SecretViewModel secret, string userName);
        void RemoveSecret(Secret secret);
        IEnumerable<Secret> GetAllByUserName(string userName);
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

        public string AddSecret(SecretViewModel secret, string userName)
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

        public void RemoveSecret(Secret secret)
        {
            _context.Secrets.Remove(secret);
            _context.SaveChanges();
        }
    }
}
