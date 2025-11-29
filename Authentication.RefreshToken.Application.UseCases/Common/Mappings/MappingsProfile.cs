using Authentication.RefreshToken.Application.Dto.Account;
using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole;
using Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser;
using Authentication.RefreshToken.Application.UseCases.User.Commands.UpdateUser;
using Authentication.RefreshToken.Domain.Entities;
using Mapster;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Auth

            config.NewConfig<RegisterCommand, Domain.Entities.User>()
            .Map(dest => dest.PasswordHash, src => src.Password)
            .IgnoreNullValues(true);

            // Role

            config.NewConfig<CreateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            config.NewConfig<UpdateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Role, RoleSummaryDto>().IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Role, RoleDto>()
                .Map(dest => dest.Users, src => src.UserRoles!
                //.Where(ur => ur.IsAssigned)
                .Select(ur => $"{ur.User.FirstName} {ur.User.LastName}")
                .ToList())
                .Map(dest => dest.Permissions, src => src.RolePermissions!
                //.Where(ur => ur.IsAssigned)
                .Select(rp => rp.Permission.Name)
                .ToList())
                .Map(dest => dest.Audit.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.Audit.CreatedBy, src => $"{src.CreatedByUser!.FirstName} {src.CreatedByUser.LastName}")
                .Map(dest => dest.Audit.UpdatedAt, src => src.UpdatedAt != null ? src.UpdatedAt : null)
                .Map(dest => dest.Audit.UpdatedBy, src => src.UpdateByUser != null
                    ? $"{src.UpdateByUser.FirstName} {src.UpdateByUser.LastName}"
                    : null)
                .Map(dest => dest.Audit.DeactivatedAt, src => src.DeactivatedAt != null ? src.DeactivatedAt : null)
                .Map(dest => dest.Audit.DeactivatedBy, src => src.DeactivatedByUser != null
                    ? $"{src.DeactivatedByUser.FirstName} {src.DeactivatedByUser.LastName}"
                    : null)
                .IgnoreNullValues(true);

            // User

            config.NewConfig<CreateUserCommand, Domain.Entities.User>()
                .Map(dest => dest.PasswordHash, src => src.Password)
                .IgnoreNullValues(true);

            config.NewConfig<UpdateUserCommand, Domain.Entities.User>()
                .Map(dest => dest.PasswordHash, src => src.Password)
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.User, UserSummaryDto>()
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.User, UserDto>()
                .Map(dest => dest.Roles, src => src.UserRoles
                //.Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList()).IgnoreNullValues(true);

            // Account
            config.NewConfig<Domain.Entities.User, AccountDto>()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName)
                .Map(dest => dest.Roles, src => src.UserRoles
                //.Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList()).IgnoreNullValues(true);

            // Permission
            config.NewConfig<Domain.Entities.Permission, PermissionDto>()
                .Map(dest => dest.Audit.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.Audit.CreatedBy, src => $"{src.CreatedByUser!.FirstName} {src.CreatedByUser.LastName}")
                .Map(dest => dest.Audit.UpdatedAt, src => src.UpdatedAt != null ? src.UpdatedAt : null)
                .Map(dest => dest.Audit.UpdatedBy, src => src.UpdateByUser != null
                    ? $"{src.UpdateByUser.FirstName} {src.UpdateByUser.LastName}"
                    : null)
                .Map(dest => dest.Audit.DeactivatedAt, src => src.DeactivatedAt != null ? src.DeactivatedAt : null)
                .Map(dest => dest.Audit.DeactivatedBy, src => src.DeactivatedByUser != null
                    ? $"{src.DeactivatedByUser.FirstName} {src.DeactivatedByUser.LastName}"
                    : null).IgnoreNullValues(true);
        }
    }
}
