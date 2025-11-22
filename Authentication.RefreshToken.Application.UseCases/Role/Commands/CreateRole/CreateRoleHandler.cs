using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, SuccessResponse<RoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<RoleDto>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<RoleDto>();
            var roleExists = await _unitOfWork.Roles.GetByNameAsync(request.Name)
                ?? throw new ConflictException($"Role {request.Name} exists");
            var roleMapped = _mapper.Map<Domain.Entities.Role>(request);
            var roleCreated = await _unitOfWork.Roles.CreateAsync(roleMapped)
                ?? throw new Exception("Error creating role");
            //_unitOfWork.CommitAsync();
            response.Data = _mapper.Map<RoleDto>(roleCreated);
            response.Message = "Role created successfully";

            return response;
        }
    }
}
