using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Authentication;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Login;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Refresh;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.Register;
using Authentication.RefreshToken.Application.UseCases.Authentication.Command.RevokeToken;
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
        [SwaggerResponse( StatusCodes.Status200OK, "OK", typeof(ApiResponse<TokenInfoDto>) )]
        [SwaggerResponse( StatusCodes.Status400BadRequest , "BadRequest" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status401Unauthorized , "Unauthorized" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status404NotFound , "Not Found" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status500InternalServerError , "Internal Server error" , typeof(ErrorResponse) )]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);

            Response.Cookies.Append(
                "refresh_token",
                response.Data.RefreshToken!,
                new CookieOptions
                {
                    HttpOnly = true,
                    //Secure = true,
                    //SameSite = SameSiteMode.Strict,
                    Secure = true,                 // Only fodev
                    SameSite = SameSiteMode.None,   // cross-site
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            return Ok(new ApiResponse<string>(
                response.Data.AccessToken,
                response.Message));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [SwaggerOperation(
            Summary = "Register a new user",
            Description = "Registers a new user with the provided details and returns authentication tokens.",
            OperationId = "RegisterUser"
        )]
        [SwaggerResponse( StatusCodes.Status201Created, "OK" , typeof(ApiResponse<TokenInfoDto>) )]
        [SwaggerResponse( StatusCodes.Status400BadRequest , "Bad Request" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status401Unauthorized , "Unauthorized" , typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status404NotFound , "Not Found", typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status409Conflict , "Conflict", typeof(ErrorResponse) )]
        [SwaggerResponse( StatusCodes.Status500InternalServerError , "Internal Server error", typeof(ErrorResponse) )]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            Response.Cookies.Append(
                "refresh_token",
                response.Data.RefreshToken!,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            return Ok(new ApiResponse<string>(
                response.Data.AccessToken,
                response.Message));
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        [SwaggerOperation(
            Summary = "Refresh authentication tokens",
            Description = "Generates new authentication tokens using the provided refresh token.",
            OperationId = "RefreshToken"
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "OK", typeof(ApiResponse<TokenInfoDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found ", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new ErrorResponse("Refresh token cookie not found."));
            }
            //+19564206724
            var response = await _mediator.Send(new RefreshCommand
            {
                RefreshToken = refreshToken
            });

            Response.Cookies.Append(
                "refresh_token",
                response.Data.RefreshToken!,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                }
            );

            return Ok(new ApiResponse<string>(
                response.Data.AccessToken,
                response.Message));
        }
        [AllowAnonymous]
        [HttpPost("revoke")]
        [SwaggerOperation(
            Summary = "Revoke refresh token"
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "OK", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found ", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> RevokeRefreshToken()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new ErrorResponse("Refresh token cookie not found."));
            }

            var response = await _mediator.Send(new RevokeTokenCommand
            {
                RefreshToken = refreshToken
            });

            Response.Cookies.Delete("refresh_token");

            return Ok(response);
        }
    }

}
