using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.CreateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.DeactivateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Commands.UpdateRole;
using Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRole;
using Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRoleWithoutPagination;
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
    [SwaggerTag("Role controller")]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/roles")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "SUPER_ADMIN")]
        [SwaggerOperation(
            Summary = "Create Role",
            Description = "Create a new role in the system",
            OperationId = "CreateRole"
        )]
        [SwaggerResponse(StatusCodes.Status201Created, "OK", typeof(ApiResponse<RoleSummaryDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRoleCommand command)
        {
            var response =  await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(
            Summary = "Deactivate Role",
            Description = "Deactivate a role by its identifier",
            OperationId = "DeactivateRole"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<object>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> DeactivateAsync(int id)
        {
            var response = await _mediator.Send(new DeactivateRoleCommand() { Id = id});
            return Ok(response);
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "List of Roles",
            Description = "Obtain all roles with pagination",
            OperationId = "ListRoles"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PagedResponse<RoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoleQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("/form")]
        [SwaggerOperation(
            Summary = "Get All Roles",
            Description = "Retrieve all roles in the system without pagination",
            OperationId = "GetAllRoles"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(PagedResponse<RoleDto>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _mediator.Send(new GetAllRoleWithoutPaginationQuery() { });
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation(
            Summary = "Get Role by Id",
            Description = "Obtain a role by its identifier",
            OperationId = "GetRoleById"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<RoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _mediator.Send(new GetRoleQuery() { Id = id});
            return Ok(response);
        }

        [HttpPatch]
        [SwaggerOperation(
            Summary = "Update Role",
            Description = "Update an existing role in the system",
            OperationId = "UpdateRole"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ApiResponse<RoleSummaryDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRoleCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
