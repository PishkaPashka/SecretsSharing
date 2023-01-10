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
            CreateMap<TextSecretViewModel, TextSecret>();

            CreateMap<FileSecret, SecretViewModel>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Path));

            CreateMap<TextSecret, SecretViewModel>()
               .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Text));
        }
    }
}
