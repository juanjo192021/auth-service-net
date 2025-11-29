using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRole
{
    public class GetRoleHandler : IRequestHandler<GetRoleQuery, ApiResponse<RoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<RoleDto>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {  
            var role = await _unitOfWork.Roles.GetByIdAsync(request.Id);
            if (role == null)
                throw new NotFoundException($"Role with ID {request.Id} not found");
            return new ApiResponse<RoleDto>(
                _mapper.Map<RoleDto>(role),
                "Role retrieved successfully");
        }
    }
}
