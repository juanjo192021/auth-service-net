using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, SuccessResponse<RoleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SuccessResponse<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<RoleDto>();
            var roleExists = await _unitOfWork.Roles.GetByNameAsync(request.Name);
            if (roleExists != null && roleExists.Id != request.Id)
                throw new ConflictException($"The role {request.Name} can't be updated because it already exists.");

            var roleMapped = _mapper.Map<Domain.Entities.Role>(request);
            var roleUpdated = await _unitOfWork.Roles.UpdateAsync(roleMapped);
            if (roleUpdated == null)
                throw new NotFoundException($"Role with ID {request.Id} not found.");

            response.Data = _mapper.Map<RoleDto>(roleUpdated);
            response.Message = "Role updated sucessfuly!";

            return response;
        }
    }
}
