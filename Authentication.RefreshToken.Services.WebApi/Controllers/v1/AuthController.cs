using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Auth;
using Authentication.RefreshToken.Application.UseCases.Auth.Command.RefreshToken;
using Authentication.RefreshToken.Application.UseCases.Auth.Command.SignIn;
using Authentication.RefreshToken.Application.UseCases.Auth.Command.SignUp;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.RefreshToken.Services.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Authentication and Authorization Controller")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        [SwaggerOperation(
            Summary = "User sign-in",
            Description = "Authenticates a user with email and password, returning authentication tokens upon success.",
            OperationId = "SignInUser"
        )]
        [SwaggerResponse(
            200,
            "User signed in successfully",
            typeof(Response<AuthDto>)
        )]
        [SwaggerResponse(
            401,
            "Unauthorized",
            typeof(Response<AuthDto>)
        )]
         [SwaggerResponse(
            500,
            "Error user",
            typeof(Response<AuthDto>)
        )]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Registers a new user with the provided details and returns authentication tokens.",
            OperationId = "SignUpUser"
        )]
        [SwaggerResponse(
            201,
            "User created successfuly",
            typeof(Response<AuthDto>)
        )]
        [SwaggerResponse(
            400,
            "Bad Request",
            typeof(Response<AuthDto>)
        )]
        [SwaggerResponse(
            500,
            "Error user",
            typeof(Response<AuthDto>)
        )]
        public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [SwaggerOperation(
            Summary = "Refresh authentication tokens",
            Description = "Generates new authentication tokens using the provided refresh token.",
            OperationId = "RefreshToken"
        )]
        [SwaggerResponse(
            200,
            "Tokens refreshed successfully",
            typeof(Response<TokenInfoDto>)
        )]
        [SwaggerResponse(
            401,
            "Unauthorized",
            typeof(Response<TokenInfoDto>)
        )]
        [SwaggerResponse(
            500,
            "Error refreshing tokens",
            typeof(Response<TokenInfoDto>)
        )]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
