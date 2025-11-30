using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetAllPermission
{
    public class GetAllPermissionHandler : IRequestHandler<GetAllPermissionQuery, ApiResponse<IEnumerable<PermissionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllPermissionHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<PermissionSummaryDto>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
        {
            var totalRecords = await _unitOfWork.Permissions.CountAsync();

            if (totalRecords < 1)
                throw new NotFoundException("Not found any permissions");

            var permissions = await _unitOfWork.Permissions.GetAllAsync();

            return new ApiResponse<IEnumerable<PermissionSummaryDto>>(
                _mapper.Map<IEnumerable<PermissionSummaryDto>>(permissions),
                "Permissions retrieved successfully");
        }
    }
}
