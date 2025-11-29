using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserSummaryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;

        public CreateUserHandler(IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<ApiResponse<UserSummaryDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (!await _unitOfWork.Users.IsEmailUniqueAsync(request.Email))
                throw new ConflictException($"The user email with {request.Email} already exists.");

            if (!await _unitOfWork.Users.IsDocumentUniqueAsync(request.DocumentNumber))
                throw new ConflictException($"The user document number {request.DocumentNumber} already exists.");

            request.Password = _passwordHasher.Hash(request.Password);

            var user = _mapper.Map<Domain.Entities.User>(request);
            var createdUser = await _unitOfWork.Users.CreateAsync(user)
                ?? throw new Exception("Error creating user");

            await _unitOfWork.CommitAsync(cancellationToken);

            if(request.Roles is not null)
            {
                await _unitOfWork.UserRoles.AssignRolesAsync(createdUser.Id, request.Roles);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return new ApiResponse<UserSummaryDto>()
            {
                Data = _mapper.Map<UserSummaryDto>(createdUser),
                Message = "User created successfully"
            };
        }
    }
}
