using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Permission;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermission;
using Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissionLookup;
using Authentication.RefreshToken.Application.UseCases.Permission.Queries.GetPermissions;
using Authentication.RefreshToken.Application.UseCases.Role.Queries.GetRole;
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
            Summary = "List of Permissions",
            Description = "Obtain all permissions with pagination",
            OperationId = "ListPermissions"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PagedResponse<PermissionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetPermissionsQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("get-permissions")]
        [SwaggerOperation(
            Summary = "Get All Permissions",
            Description = "Retrieve all permissions in the system",
            OperationId = "GetAllPermissions"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<IEnumerable<PermissionSummaryDto>>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _mediator.Send(new GetPermissionLookupQuery() { });
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get Permission by Id",
            Description = "Obtain a permission by its identifier",
            OperationId = "GetPermissionById"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<PermissionDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _mediator.Send(new GetPermissionQuery() { Id = id });
            return Ok(response);
        }
    }
}
