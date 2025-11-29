using Authentication.RefreshToken.Application.Dto.Account;
using Authentication.RefreshToken.Application.Interfaces.Infrastructure.Security;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Authentication.RefreshToken.Application.UseCases.Account.Queries.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, ApiResponse<AccountDto>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCurrentUserHandler(
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<AccountDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
                throw new Exception("User is not authenticated.");
            var user = await _unitOfWork.Users.GetByIdAsync(userId.Value);

            if (user == null)
                throw new NotFoundException("User not found.");

            return new ApiResponse<AccountDto>(_mapper.Map<AccountDto>(user), "Current user retrieved successfully.");
        }
    }
}
