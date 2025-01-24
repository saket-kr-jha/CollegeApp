using AutoMapper;
using DotNetCore_New.Model;
using DotNetCore_New.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private APIResponse _apiResponse;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _apiResponse = new();
            _logger = logger;
        }
        [HttpPost]
        [Route("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateUserAsync(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.StatusMessage = "User data is required";
                    return _apiResponse;
                }
                var result = await _userService.CreateUserAsync(userDTO);
                if (result)
                {
                    _apiResponse.StatusCode = HttpStatusCode.Created;
                    _apiResponse.Status = true;
                    _apiResponse.StatusMessage = "User created successfully";
                    _apiResponse.Data = result;
                    //return CreatedAtRoute("GetUserByID", new { id = userDTO.Id }, _apiResponse);
                    return Ok(_apiResponse);
                }
                else
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.StatusMessage = "User creation failed";
                    return _apiResponse;
                }
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.StatusMessage = ex.Message;
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("All", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetUsersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userService.GetUsersAsync();
                _apiResponse.Data = users;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.StatusMessage = "Users fetched successfully";
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.StatusMessage = ex.Message;
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetUserByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogError("Invalid user id");
                    return BadRequest();
                }
                _logger.LogInformation("Fetching user by id");
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) {
                    _logger.LogError("User not found");
                    return NotFound("User not found");
                }
                _apiResponse.Data = user;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.StatusMessage = "User fetched successfully";
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.StatusMessage = ex.Message;
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetUserByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetUserByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogError("Invalid user name");
                    return BadRequest();
                }
                _logger.LogInformation("Fetching user by name");
                var user = await _userService.GetUserByNameAsync(name);
                if (user == null)
                {
                    _logger.LogError("User not found");
                    return NotFound("User not found");
                }
                _apiResponse.Data = user;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.StatusMessage = "User fetched successfully";
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.StatusMessage = ex.Message;
                return _apiResponse;
            }
        }

    }
}
