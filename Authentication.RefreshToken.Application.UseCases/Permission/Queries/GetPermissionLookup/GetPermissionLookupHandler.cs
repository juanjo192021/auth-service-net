using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissionLookup
{
    public class GetPermissionLookupHandler : IRequestHandler<GetPermissionLookupQuery, ApiResponse<IEnumerable<PermissionSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetPermissionLookupHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<PermissionSummaryDto>>> Handle(GetPermissionLookupQuery request, CancellationToken cancellationToken)
        {
            var totalRecords = await _unitOfWork.Permissions.CountAsync();

            if (totalRecords < 1)
                throw new NotFoundException("Not found any permissions");

            var permissions = await _unitOfWork.Permissions.GetLookupAsync();

            return new ApiResponse<IEnumerable<PermissionSummaryDto>>(
                _mapper.Map<IEnumerable<PermissionSummaryDto>>(permissions),
                "Permissions retrieved successfully");
        }
    }
}
