using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.DeactivateRole
{
    public class DeactivateRoleHandler : IRequestHandler<DeactivateRoleCommand, SuccessResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeactivateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SuccessResponse<bool>> Handle(DeactivateRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new SuccessResponse<bool>();
            var isDeactivated = await _unitOfWork.Roles.DeactivateAsync(request.Id);
            if (!isDeactivated)
                throw new NotFoundException($"Not found role with ID {request.Id}");
            
            //response.Data = isDeactivated;
            response.Message = "Role deactivated successfully.";
            return response;
        }
    }
}
