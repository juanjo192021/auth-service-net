using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Queries.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, ApiResponse<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserDto>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException($"User with ID {request.Id} not found");
            return new ApiResponse<UserDto>()
            {
                Data = _mapper.Map<UserDto>(user),
                Message = "Role retrieved successfully"
            };
        }
    }
}
