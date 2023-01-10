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
    /// <summary>
    /// A service that provides an interface for adding, getting and removing secrets from db.
    /// </summary>
    public interface ISecretObjectService
    {
        /// <summary>
        /// Add text secret in db
        /// </summary>
        /// <param name="secret">ViewModel with data, filled by user</param>
        /// <param name="userName">Current user</param>
        /// <returns>Id of secret</returns>
        string Add(TextSecretViewModel secret, string userName);

        /// <summary>
        /// Add file secret in db
        /// </summary>
        /// <param name="path">Path to the file</param>
        /// <param name="fileName">Name of file with extension</param>
        /// <param name="isOneUse">Should the secret be deleted after being viewed once</param>
        /// <param name="userName">Current user</param>
        /// <returns>Id of secret</returns>
        string Add(string path, string fileName, bool isOneUse, string userName);

        /// <summary>
        /// Get all secrets for current users
        /// </summary>
        /// <param name="userName">Current user</param>
        /// <returns>All secrets of current user</returns>
        IEnumerable<SecretViewModel> GetAllByUserName(string userName);

        /// <summary>
        /// Get text secred by id
        /// </summary>
        /// <param name="id">Identifier of secret</param>
        /// <returns>Secret text</returns>
        string GetTextById(string id);

        /// <summary>
        /// Get file by id
        /// </summary>
        /// <param name="id">Identifier of secret</param>
        /// <returns>Secret file</returns>
        FileSecret GetFileById(string id);

        /// <summary>
        /// Delete secret by id
        /// </summary>
        /// <param name="id">Identifier of secret</param>
        /// <param name="userName">Current user</param>
        /// <returns>True if deleting is successful, false if not</returns>
        bool DeleteById(string id, string userName);

        /// <summary>
        /// Dekete secret by entity
        /// </summary>
        /// <typeparam name="TEntity">Generic type, can be TextSecret or FileSecret</typeparam>
        /// <param name="entity">Entity for removing</param>
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

        public string GetTextById(string id)
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
