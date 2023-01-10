using AutoMapper;
using SecretsSharing.Models;
using SecretsSharing.Models.Secrets;
using SecretsSharing.ViewModels;
using System;

namespace SecretsSharing.ObjectServices
{
    public interface ISecretObjectService
    {
        string AddSecret(SecretViewModel secret);
        void RemoveSecret(Secret secret);
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

        public string AddSecret(SecretViewModel secret)
        {
            var entity = _mapper.Map<Secret>(secret);

            var entityId = Guid.NewGuid().ToString();
            entity.Id = entityId;

            _context.TextSecrets.Add(entity);
            _context.SaveChanges();

            return entityId;
        }

        public void RemoveSecret(Secret secret)
        {
            _context.TextSecrets.Remove(secret);
            _context.SaveChanges();
        }
    }
}
