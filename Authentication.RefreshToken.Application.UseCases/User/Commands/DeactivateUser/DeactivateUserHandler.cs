using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.DeactivateUser
{
    public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand, ApiResponse<object>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeactivateUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<object>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var wasDeactivated = await _unitOfWork.Users.DeactivateAsync(request.Id);
            if (!wasDeactivated)
                throw new NotFoundException($"User with ID {request.Id} not found");

            await _unitOfWork.CommitAsync(cancellationToken);
            return new ApiResponse<object>
            {
                Message = "User deactivated successfully."
            };
        }
    }
}
