using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.Interfaces.Persistence;
using Authentication.RefreshToken.Application.UseCases.Common.Exceptions;
using Authentication.RefreshToken.Concerns.Common;
using MapsterMapper;
using MediatR;

namespace Authentication.RefreshToken.Application.UseCases.User.Queries.GetAllUser
{
    public class GetAllUserHandler : IRequestHandler<GetAllUserQuery, PagedResponse<IEnumerable<UserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<UserDto>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;
            var search = request.Search;

            var totalRecords = await _unitOfWork.Users.CountAsync();
            if (totalRecords < 1)
                throw new NotFoundException("Not found any users");

            var users = await _unitOfWork.Users.GetAllAsync(pageNumber, pageSize, search);

            return new PagedResponse<IEnumerable<UserDto>>(
                _mapper.Map<IEnumerable<UserDto>>(users),
                "Users retrieved successfully",
                pageNumber,
                pageSize,
                totalRecords);
        }
    }
}
