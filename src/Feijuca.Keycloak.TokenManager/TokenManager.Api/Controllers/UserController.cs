using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenManager.Application.Services.Commands.Users;
using TokenManager.Application.Services.Requests.User;

namespace TokenManager.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize(Policy = "TokenManager")]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Add a new Category on the platform.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("addUser", Name = nameof(AddUser))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser([FromBody] AddUserRequest addUserRequest, CancellationToken cancellationToken)
        {
            var categoryId = await _mediator.Send(new CreateUserCommand(addUserRequest), cancellationToken);
            return Created("/addCategory", categoryId);
        }
    }
}
