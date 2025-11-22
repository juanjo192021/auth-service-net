using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Domain.Entities;
using Mapster;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // De un User a un UserDTO
            config.NewConfig<User, UserInfoDto>()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName)
                .Map(dest => dest.Roles, src => src.UserRoles
                .Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList()).IgnoreNullValues(true);

            config.NewConfig<RegisterDto, User>()
            .Map(dest => dest.PasswordHash, src => src.Password)
            .IgnoreNullValues(true);
        }
    }
}
