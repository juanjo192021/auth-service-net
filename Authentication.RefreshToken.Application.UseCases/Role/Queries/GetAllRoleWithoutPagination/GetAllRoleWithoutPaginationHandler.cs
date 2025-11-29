using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRoleWithoutPagination
{
    public class GetAllRoleWithoutPaginationHandler : IRequestHandler<GetAllRoleWithoutPaginationQuery, ApiResponse<IEnumerable<RoleSummaryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllRoleWithoutPaginationHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<RoleSummaryDto>>> Handle(GetAllRoleWithoutPaginationQuery request, CancellationToken cancellationToken)
        {
            var totalRecords = await _unitOfWork.Roles.CountAsync();

            if (totalRecords < 1)
                throw new NotFoundException("Not found any roles");

            var roles = await _unitOfWork.Permissions.GetAllAsync();

            return new ApiResponse<IEnumerable<RoleSummaryDto>>(
                _mapper.Map<IEnumerable<RoleSummaryDto>>(roles),
                "Roles retrieved successfully");
        }
    }
}
