using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register;
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
    [Route("api/v{version:apiVersion}/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "User login",
            Description = "Authenticates a user with email and password, returning authentication tokens upon success.",
            OperationId = "LoginUser"
        )]
        [SwaggerResponse( StatusCodes.Status200OK, "OK", typeof(SuccessResponse<AuthenticationDto>) )]
        [SwaggerResponse( StatusCodes.Status400BadRequest , "BadRequest" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status401Unauthorized , "Unauthorized" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status404NotFound , "Not Found" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status500InternalServerError , "Internal Server error" , typeof(ErrorResponse) )]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Registers a new user with the provided details and returns authentication tokens.",
            OperationId = "RegisterUser"
        )]
        [SwaggerResponse( StatusCodes.Status200OK , "OK" , typeof(SuccessResponse<AuthenticationDto>) )]
        [SwaggerResponse( StatusCodes.Status400BadRequest , "BadRequest" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status401Unauthorized , "Unauthorized" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status404NotFound , "Not Found", typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status409Conflict , "Conflict", typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status500InternalServerError , "Internal Server error", typeof(ErrorResponse) )]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [SwaggerOperation(
            Summary = "Refresh authentication tokens",
            Description = "Generates new authentication tokens using the provided refresh token.",
            OperationId = "RefreshToken"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(SuccessResponse<TokenInfoDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found ", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> Refresh([FromBody] RefreshCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
