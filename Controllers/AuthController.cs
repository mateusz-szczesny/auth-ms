using System;
using System.Threading.Tasks;
using Auth.Requests;
using Auth.Responses;
using Auth.Services;
using Auth.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly LoginRequestValidator _loginRequestValidator;
        private readonly SignUpRequestValidator _signUpRequestValidator;
        public AuthController(
            IAuthService authService,
            LoginRequestValidator loginRequestValidator,
            SignUpRequestValidator signUpRequestValidator)
        {
            _authService = authService;
            _loginRequestValidator = loginRequestValidator;
            _signUpRequestValidator = signUpRequestValidator;
        }

        /// <summary>
        /// Endpoint for requesting for access token
        /// </summary>
        /// <response code="200">Ok - successful</response>
        /// <response code="400">Bad Request - error during request(Error in message)</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorPayload), 400)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validationResult = _loginRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var token = await _authService.Login(request.Identifier, request.Password);
                return Ok(new TokenResponse { Token = token });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorPayload(1, e.Message));
            }
        }

        /// <summary>
        /// Endpoint for sign up new user
        /// </summary>
        /// <response code="200">Ok - successful</response>
        /// <response code="400">Bad Request - error during request(Error in message)</response>
        [HttpPost("signup")]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorPayload), 400)]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var validationResult = _signUpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }
            try
            {
                await _authService.Register(request.Username, request.Email, request.Password);
                return CreatedAtAction(nameof(Login), new { identifier = request.Username, password = request.Password }, request);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorPayload(1, e.Message));
            }
        }

        /// <summary>
        /// Endpoint for checking authorization status
        /// </summary>
        /// <response code="200">Ok - authorizated</response>
        /// <response code="401">Unauthorized - unauthorized</response>
        [HttpGet("permission")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Permission()
        {
            return Ok();
        }
    }
}