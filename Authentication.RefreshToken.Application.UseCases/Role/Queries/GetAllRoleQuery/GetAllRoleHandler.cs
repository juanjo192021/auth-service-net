using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRoleQuery
{
    public class GetAllRoleHandler : IRequestHandler<GetAllRoleQuery, ResponsePagination<IEnumerable<RoleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponsePagination<IEnumerable<RoleDto>>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            var response = new ResponsePagination<IEnumerable<RoleDto>>();

            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;
            var search = request.Search;

            var count = await _unitOfWork.Roles.CountAsync();
            if (count < 1)
                throw new NotFoundException("Not found any roles");

            var roles = await _unitOfWork.Roles.GetAllAsync(pageNumber, pageSize, search);

            response.Data = _mapper.Map<IEnumerable<RoleDto>>(roles);
            response.Message = "Roles retrieved successfully";
            response.PageNumber = pageNumber;
            response.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            response.TotalCount = count;

            return response;
        }
    }
}
