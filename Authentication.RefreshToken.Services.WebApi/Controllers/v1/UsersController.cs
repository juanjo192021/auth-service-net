using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.User;
using Authentication.RefreshToken.Application.UseCases.User.Commands.CreateUser;
using Authentication.RefreshToken.Application.UseCases.User.Commands.DeactivateUser;
using Authentication.RefreshToken.Application.UseCases.User.Commands.UpdateUser;
using Authentication.RefreshToken.Application.UseCases.User.Queries.GetAllUser;
using Authentication.RefreshToken.Application.UseCases.User.Queries.GetUser;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.RefreshToken.Services.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [SwaggerTag("User controller")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Create User",
            Description = "Create a new user in the system",
            OperationId = "CreateUser"
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "OK", typeof(ApiResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Deactivate User",
            Description = "Deactivate an existing user in the system",
            OperationId = "DeactivateUser"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<bool>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> DeactivateAsync(int id)
        {
            var response = await _mediator.Send(new DeactivateUserCommand() { Id = id });
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get All Users",
            Description = "Retrieve a paginated list of all users in the system",
            OperationId = "GetAllUsers"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PagedResponse<UserSummaryDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllUserQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get User By Id",
            Description = "Retrieve a user by their unique identifier",
            OperationId = "GetUserById"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<UserSummaryDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _mediator.Send(new GetUserQuery() { Id = id });
            return Ok(response);
        }

        [HttpPatch]
        [SwaggerOperation(
            Summary = "Update User",
            Description = "Update an existing user in the system",
            OperationId = "UpdateUser"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<UserDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
