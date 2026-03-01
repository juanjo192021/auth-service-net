using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermission
{
    public class GetPermissionHandler : IRequestHandler<GetPermissionQuery, ApiResponse<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PermissionDto>> Handle(GetPermissionQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.Permissions.GetByIdAsync(request.Id);
            if (permission == null)
                throw new NotFoundException($"Permission with ID {request.Id} not found");
            return new ApiResponse<PermissionDto>(
                _mapper.Map<PermissionDto>(permission),
                "Permission retrieved successfully");
        }
    }
}
