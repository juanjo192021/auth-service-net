using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, ApiResponse<RoleSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<RoleSummaryDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Roles.FindByIdAsync(request.Id)
                ?? throw new NotFoundException($"Role with ID {request.Id} not found.");

            if (request.Name is not null && role.Name != request.Name)
            {
                if (!await _unitOfWork.Roles.IsNameUniqueAsync(request.Name))
                    throw new ConflictException($"Another role with name '{request.Name}' already exists.");
            }

            _mapper.Map(request, role);

            var updatedRole = await _unitOfWork.Roles.UpdateAsync(role);

            if (request.PermissionIds is not null)
            {
                await _unitOfWork.RolePermissions.UpdatePermissionsAsync(role.Id, request.PermissionIds);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<RoleSummaryDto>(
                _mapper.Map<RoleSummaryDto>(updatedRole!),
                "Role updated sucessfuly!");
        }
    }
}
