using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole;
using Authentication.RefreshToken.Domain.Entities;
using Mapster;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Auth
            config.NewConfig<User, UserInfoDto>()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName)
                .Map(dest => dest.Roles, src => src.UserRoles
                .Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList()).IgnoreNullValues(true);

            config.NewConfig<RegisterDto, User>()
            .Map(dest => dest.PasswordHash, src => src.Password)
            .IgnoreNullValues(true);

            // Role
            config.NewConfig<CreateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            config.NewConfig<UpdateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Role, RoleDto>()
                .Map(dest => dest.Users, src => src.UserRoles!
                .Where(ur => ur.IsAssigned)
                .Select(ur => ur.User.FirstName + " " + ur.User.LastName)
                .ToList())
                .IgnoreNullValues(true);
        }
    }
}
