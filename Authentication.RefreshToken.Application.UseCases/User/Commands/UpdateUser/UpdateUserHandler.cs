using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ApiResponse<UserSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserHandler(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<UserSummaryDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _unitOfWork.Users.FindByIdAsync(request.Id)
                ?? throw new NotFoundException($"User with ID {request.Id} not found.");

            if (request.Email is not null && request.Email != user.Email)
            {
                if (!await _unitOfWork.Users.IsEmailUniqueAsync(request.Email))
                    throw new ConflictException($"Another user with email '{request.Email}' already exists.");
            }

            if(request.DocumentNumber is not null && request.DocumentNumber != user.DocumentNumber)
            {
                if (!await _unitOfWork.Users.IsDocumentUniqueAsync(request.DocumentNumber))
                    throw new ConflictException($"Another user with document number '{request.DocumentNumber}' already exists.");
            }
            
            request.Password = _passwordHasher.Hash(request.Password);

            _mapper.Map(request, user);

            var updatedUser = await _unitOfWork.Users.UpdateAsync(user);

            if (request.Roles is not null)
            {
                await _unitOfWork.UserRoles.UpdateRolesAsync(user.Id, request.Roles);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<UserSummaryDto>
            {
                Data = _mapper.Map<UserSummaryDto>(updatedUser!),
                Message = "User updated sucessfully!"
            };
        }
    }
}
