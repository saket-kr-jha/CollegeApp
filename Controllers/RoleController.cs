using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Data.Repository;
using DotNetCore_New.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private APIResponse _apiResponse;
        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository) { 
            _mapper = mapper;
            _roleRepository = roleRepository;
            _apiResponse = new();
        }
        [HttpPost]
        [Route("CreateRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<APIResponse>> CreateRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                if(roleDTO == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.StatusMessage = "Role data is required";
                    return _apiResponse;
                }
                Role role = _mapper.Map<Role>(roleDTO);
                role.IsDeleted = false;
                role.CreatedDate = DateTime.Now;
                role.ModifiedDate = DateTime.Now;
                
                var result = await _roleRepository.CreateAsync(role);
                roleDTO.Id = result.Id;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Status = true;
                _apiResponse.StatusMessage = "Role created successfully";
                _apiResponse.Data = result;

                //return Ok(_apiResponse);
                return CreatedAtRoute("GetRoleByID", new { id = roleDTO.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                 return _apiResponse;
            }
        }
        [HttpGet]
        [Route("All", Name ="GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<APIResponse>> GetRolesAsync()
        {
            try {
                var roles = await _roleRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<RoleDTO>>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetRoleById(int id)
        {
            try
            {
                if(id<= 0)
                {
                    return BadRequest();
                }

                var role = await _roleRepository.GetAsync(role => role.Id == id);
                if(role == null)
                {
                    return NotFound($"Requested Role with Role Id : {id} does not Exist");
                }
                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpGet]
        [Route("{name:alpha}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetRoleByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest();
                }

                var role = await _roleRepository.GetAsync(role => role.RoleName.ToLower().Contains(name));
                if (role == null)
                {
                    return NotFound($"Requested Role with Role Name : {name} does not Exist");
                }
                _apiResponse.Data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateRoleAsync(RoleDTO roleDTO)
        {
            try
            {
                if(roleDTO == null | roleDTO.Id <= 0)
                {
                    return BadRequest();
                }
                var existingRole = await _roleRepository.GetAsync(role => role.Id == roleDTO.Id, true);

                if(existingRole == null)
                {
                    return BadRequest($"Role not found with id: {roleDTO.Id} to update");
                }

                var newRole = _mapper.Map<Role>(roleDTO);
                await _roleRepository.UpdateAsync(newRole);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = newRole;
                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
        [HttpDelete]
        [Route("Delete/{Id}", Name = "DeleteRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRoleAsync(int Id)
        {
            try
            {
                if(Id <= 0)
                {
                    return BadRequest();
                }
                var role = await _roleRepository.GetAsync(role => role.Id == Id);
                if (role == null)
                    return BadRequest($"Role not found with id : {Id} to delete");

                await _roleRepository.DeleteAsync(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
            }
        }
    }
}
