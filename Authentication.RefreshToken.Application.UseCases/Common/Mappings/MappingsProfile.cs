using Authentication.RefreshToken.Application.Dto.Account;
using Authentication.RefreshToken.Application.Dto.Common;
using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole;
using Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser;
using Authentication.RefreshToken.Application.UseCases.User.Commands.UpdateUser;
using Mapster;

namespace Authentication.RefreshToken.Application.UseCases.Common.Mappings
{
    public class MappingsProfile : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Audit
            config.NewConfig<Domain.Entities.Role, AuditDto>()
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                // Pasamos la entidad completa. Mapster la transformará a UserSummaryDto
                .Map(dest => dest.CreatedBy, src => src.CreatedByUser)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.UpdatedBy, src => src.UpdateByUser)
                .Map(dest => dest.DeactivatedAt, src => src.DeactivatedAt)
                .Map(dest => dest.DeactivatedBy, src => src.DeactivatedByUser)
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.User, AuditDto>()
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.CreatedBy, src => src.CreatedByUser)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.UpdatedBy, src => src.UpdateByUser)
                .Map(dest => dest.DeactivatedAt, src => src.DeactivatedAt)
                .Map(dest => dest.DeactivatedBy, src => src.DeactivatedByUser)
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Permission, AuditDto>()
                .Map(dest => dest.CreatedAt, src => src.CreatedAt)
                .Map(dest => dest.CreatedBy, src => src.CreatedByUser)
                .Map(dest => dest.UpdatedAt, src => src.UpdatedAt)
                .Map(dest => dest.UpdatedBy, src => src.UpdateByUser)
                .Map(dest => dest.DeactivatedAt, src => src.DeactivatedAt)
                .Map(dest => dest.DeactivatedBy, src => src.DeactivatedByUser)
                .IgnoreNullValues(true);


            // Authentication

            config.NewConfig<RegisterCommand, Domain.Entities.User>()
                .Map(dest => dest.PasswordHash, src => src.Password)
                .IgnoreNullValues(true);

            // Role

            config.NewConfig<CreateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            config.NewConfig<UpdateRoleCommand, Domain.Entities.Role>()
                .IgnoreNullValues(true);

            //config.NewConfig<Domain.Entities.Role, RoleSummaryDto>()
            //    .Map(dest => dest.Permissions, src => src.RolePermissions.Select(rp => rp.Permission))
            //    .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Role, RoleDto>()
                .Map(dest => dest.Users, src => src.UserRoles!
                //.Where(ur => ur.IsAssigned)
                .Select(ur => ur.User)
                .ToList())
                .Map(dest => dest.Permissions, src => src.RolePermissions!
                //.Where(ur => ur.IsAssigned)
                .Select(rp => rp.Permission)
                .ToList())
                .Map(dest => dest.Audit, src => src)
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
                .Select(ur => ur.Role)
                .ToList())
                .Map(dest => dest.Audit, src => src)
                .IgnoreNullValues(true);

            // Permission

            config.NewConfig<Domain.Entities.Permission, PermissionSummaryDto>()
                .IgnoreNullValues(true);

            config.NewConfig<Domain.Entities.Permission, PermissionDto>()
                .Map(dest => dest.Audit, src => src)
                .IgnoreNullValues(true);

            // Account
            config.NewConfig<Domain.Entities.User, AccountDto>()
                .Map(dest => dest.FullName, src => src.FirstName + " " + src.LastName)
                .Map(dest => dest.Roles, src => src.UserRoles
                //.Where(ur => ur.IsAssigned)
                .Select(ur => ur.Role.Name)
                .ToList()).IgnoreNullValues(true);
        }
    }
}
