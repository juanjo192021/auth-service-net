using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, ApiResponse<RoleSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<RoleSummaryDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Roles.IsNameUniqueAsync(request.Name.Trim()))
                throw new ConflictException($"The role {request.Name} already exists.");

            var role = _mapper.Map<Domain.Entities.Role>(request);
            var createdRole = await _unitOfWork.Roles.CreateAsync(role)
                ?? throw new Exception("Error creating role");
            
            await _unitOfWork.CommitAsync(cancellationToken);

            var asignedPermissions = await _unitOfWork.RolePermissions.AssignPermissionAsync(createdRole.Id, request.PermissionIds);

            if (asignedPermissions.Count != request.PermissionIds.Count)
                throw new Exception("Error assigning permissions to role");

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<RoleSummaryDto>(
                _mapper.Map<RoleSummaryDto>(createdRole),
                "Role created successfully");
        }
    }
}
