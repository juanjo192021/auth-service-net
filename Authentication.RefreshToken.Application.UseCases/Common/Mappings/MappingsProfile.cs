using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Domain.Entities;
using Mapster;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // De un User a un UserDTO
            config.NewConfig<User, UserDto>()
                .Map(dest => dest.Roles, src => src.UserRoles
                .Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList());

            config.NewConfig<SignupDto, User>()
            .Map(dest => dest.PasswordHash, src => src.Password)
            .IgnoreNullValues(true);
        }
    }
}
