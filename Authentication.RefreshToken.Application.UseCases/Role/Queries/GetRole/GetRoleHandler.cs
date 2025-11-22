using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRole
{
    public class GetRoleHandler : IRequestHandler<GetRoleQuery, SuccessResponse<RoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<RoleDto>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<RoleDto>();
            var roleFinded = await _unitOfWork.Roles.GetByIdAsync(request.Id);
            if (roleFinded == null)
                throw new NotFoundException($"Role with ID {request.Id} not found");
            response.Data = _mapper.Map<RoleDto>(roleFinded);
            response.Message = "Role retrieved successfully";
            return response;
        }
    }
}
