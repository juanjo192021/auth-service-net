using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.Role.Commands.DeactivateRole
{
    public class DeactivateRoleHandler : IRequestHandler<DeactivateRoleCommand, ApiResponse<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DeactivateRoleHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<object>> Handle(DeactivateRoleCommand request, CancellationToken cancellationToken)
        {
            var wasDeactivated = await _unitOfWork.Roles.DeactivateAsync(request.Id);

            if (!wasDeactivated)
                throw new NotFoundException($"Not found role with ID {request.Id}");

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<object>(null,"Role deactivated successfully.");
        }
    }
}
