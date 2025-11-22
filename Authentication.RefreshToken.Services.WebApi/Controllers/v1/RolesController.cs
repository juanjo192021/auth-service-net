using Asp.Versioning;
using Authentication.RefreshToken.Application.Dto.Role;
using Authentication.RefreshToken.Application.UseCases.Role.Queries.GetAllRoleQuery;
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

        [HttpGet]
        [SwaggerOperation(
            Summary = "List of Roles",
            Description = "Obtain all roles with pagination",
            OperationId = "ListRoles"
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "OK", typeof(ResponsePagination<RoleDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "BadRequest", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found", typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server error", typeof(ErrorResponse))]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoleQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }
    }
}
