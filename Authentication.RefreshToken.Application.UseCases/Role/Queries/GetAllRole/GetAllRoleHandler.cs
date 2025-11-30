using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRole
{
    public class GetAllRoleHandler : IRequestHandler<GetAllRoleQuery, PagedResponse<IEnumerable<RoleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<RoleDto>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;
            var search = request.Search;

            var totalRecords = await _unitOfWork.Roles.CountAsync();
            if (totalRecords < 1)
                throw new NotFoundException("Not found any roles");

            var roles = await _unitOfWork.Roles.GetAllAsync(pageNumber, pageSize, search);

            return new PagedResponse<IEnumerable<RoleDto>>(
                _mapper.Map<IEnumerable<RoleDto>>(roles),
                "Roles retrieved successfully",
                pageNumber,
                pageSize,
                totalRecords)
            ;
        }
    }
}
