using Contracts.Web.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenManager.Application.Services.Commands.Users;
using TokenManager.Application.Services.Requests.User;
using TokenManager.Application.Services.Responses;

namespace TokenManager.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Add a new user on the keycloak realm.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("createUser/{tenant}", Name = nameof(CreateUser))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Policy = "TokenManager")]
        //[RequiredScope("tokenmanager-read")]
        public async Task<IActionResult> CreateUser([FromRoute] string tenant, [FromBody] AddUserRequest addUserRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateUserCommand(tenant, addUserRequest), cancellationToken);

            var response = new ResponseResult<string>();
            if (result.IsSuccess)
            {
                response.Result = addUserRequest.Username;
                response.DetailMessage = "User created successfully";
                return Created("/createUser", response);
            }

            response.Result = "Some error occured while trying executing the operation";
            response.DetailMessage = result.Error.Description;
            return BadRequest(response);
        }

        /// <summary>
        /// Return a valid JWT token and details about them.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("login/{tenant}", Name = nameof(Login))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> Login([FromRoute] string tenant, [FromBody] LoginUserRequest loginUserRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LoginUserCommand(tenant, loginUserRequest), cancellationToken);

            var response = new ResponseResult<TokenResponse>();
            if (result.IsSuccess)
            {
                response.Result = result.Value;
                response.DetailMessage = "Token generated with succesfully";
                return Ok(response);
            }

            response.DetailMessage = result.Error.Description;
            return BadRequest(response);
        }
    }
}
