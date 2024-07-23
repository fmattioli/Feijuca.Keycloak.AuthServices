using MediatR;
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
        public async Task<IActionResult> CreateUser([FromRoute] string tenant, [FromBody] AddUserRequest addUserRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateUserCommand(tenant, addUserRequest), cancellationToken);
            
            if (result.IsSuccess)
            {
                var response = ResponseResult<string>.Success("User created successfully");
                return Created("/createUser", response);
            }

            var responseError = ResponseResult<string>.Failure(result.Error);
            return BadRequest(responseError);
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
            
            if (result.IsSuccess)
            {
                var response = ResponseResult<TokenDetailsResponse>.Success(result.Result);
                return Ok(response);
            }

            var responseError = ResponseResult<TokenDetailsResponse>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Return a valid JWT token and details about them refreshed.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("refreshToken/{tenant}", Name = nameof(RefreshToken))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromRoute] string tenant, [FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RefreshTokenCommand(tenant, refreshToken), cancellationToken);

            if (result.IsSuccess)
            {
                var response = ResponseResult<TokenDetailsResponse>.Success(result.Result);
                return Ok(response);
            }

            var responseError = ResponseResult<TokenDetailsResponse>.Failure(result.Error);
            return BadRequest(responseError);
        }
    }
}
