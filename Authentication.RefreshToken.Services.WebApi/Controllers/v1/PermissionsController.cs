using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetAllPermission;
using Authentication.RefreshToken.Concerns.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Authentication.RefreshToken.Services.WebApi.Controllers.v1
{
    [Authorize]
    [ApiController]
    [SwaggerTag("Permission controller")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/permissions")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Get All Permissions",
            Description = "Retrieve all permissions in the system",
            OperationId = "GetAllPermissions"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<IEnumerable<PermissionDto>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _mediator.Send(new GetAllPermissionQuery() { });
            return Ok(response);
        }
    }
}
