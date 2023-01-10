using AutoMapper;
using SecretsSharing.Models.Secrets;
using SecretsSharing.ViewModels;

namespace SecretsSharing.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureMappings();
        }

        private void ConfigureMappings()
        {
            CreateMap<SecretViewModel, Secret>();
            CreateMap<Secret, SecretViewModel>();
        }
    }
}
