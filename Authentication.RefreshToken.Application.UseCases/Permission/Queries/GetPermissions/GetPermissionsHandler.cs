using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissions
{
    public class GetPermissionsHandler : IRequestHandler<GetPermissionsQuery, PagedResponse<IEnumerable<PermissionDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPermissionsHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<PermissionDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;
            var search = request.Search;

            var totalRecords = await _unitOfWork.Permissions.CountAsync();

            if (totalRecords < 1)
                throw new NotFoundException("Not found any permissions");

            var permissions = await _unitOfWork.Permissions.GetAllAsync(pageNumber, pageSize, search);

            return new PagedResponse<IEnumerable<PermissionDto>>(
                _mapper.Map<IEnumerable<PermissionDto>>(permissions),
                "Permissions retrieved successfully",
                pageNumber,
                pageSize,
                totalRecords);
        }
    }
}
