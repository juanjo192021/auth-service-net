using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Account;
using Authentication.RefreshToken.Application.UseCases.Account.Queries.GetCurrentUser;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.RefreshToken.Services.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Account controller")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("me")]
        [SwaggerOperation(
            Summary = "Get Account by AccessToken",
            Description = "Retrieve account details by its identifier",
            OperationId = "GetAccountInformation"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<AccountDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetByAccessTokenAsync()
        {
            var response = await _mediator.Send(new GetCurrentUserQuery() { });
            return Ok(response);
        }
    }
}
