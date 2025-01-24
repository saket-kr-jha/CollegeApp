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
    public class RolePrivilageController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilage> _rolePrivilageRepository;
        private APIResponse _apiResponse;
        public RolePrivilageController(IMapper mapper, ICollegeRepository<RolePrivilage> rolePrivilageRepository)
        {
            _mapper = mapper;
            _apiResponse = new();
            _rolePrivilageRepository = rolePrivilageRepository;
        }

        [HttpPost]
        [Route("CreateRolePrivilage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<APIResponse>> CreateRolePrivilageAsync(RolePrivilageDTO dto)
        {
            try
            {
                if (dto == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                    _apiResponse.Status = false;
                    _apiResponse.StatusMessage = "Role data is required";
                    return _apiResponse;
                }
                RolePrivilage rolePrivilage = _mapper.Map<RolePrivilage>(dto);
                rolePrivilage.IsDeleted = false;
                rolePrivilage.CreatedDate = DateTime.Now;
                rolePrivilage.ModifiedDate = DateTime.Now;

                var result = await _rolePrivilageRepository.CreateAsync(rolePrivilage);
                dto.Id = result.Id;
                _apiResponse.StatusCode = HttpStatusCode.Created;
                _apiResponse.Status = true;
                _apiResponse.StatusMessage = "RolePrivilage created successfully";
                _apiResponse.Data = result;

                return Ok(_apiResponse);
                //return CreatedAtRoute("GetRolePrivilageByID", new { id = dto.Id }, _apiResponse);
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
        [Route("All", Name = "GetAllRolePrivilages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<ActionResult<APIResponse>> GetRolesPrivilageAsync()
        {
            try
            {
                var rolePrivilages = await _rolePrivilageRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<RolePrivilageDTO>>(rolePrivilages);
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
        [Route("{id:int}", Name = "GetRolePrivilageById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetRolePrivilageById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var rolePrivilage = await _rolePrivilageRepository.GetAsync(rolePrivilage => rolePrivilage.Id == id);
                if (rolePrivilage == null)
                {
                    return NotFound($"Requested Role with Role Id : {id} does not Exist");
                }
                _apiResponse.Data = _mapper.Map<RolePrivilageDTO>(rolePrivilage);
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
        [Route("{name:alpha}", Name = "GetRolePrivilageByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<APIResponse>> GetRolePrivilageByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest();
                }

                var rolePrivilage = await _rolePrivilageRepository.GetAsync(rolePrivilage => rolePrivilage.RolePrivilageName.ToLower().Contains(name));
                if (rolePrivilage == null)
                {
                    return NotFound($"Requested Role with Role Name : {name} does not Exist");
                }
                _apiResponse.Data = _mapper.Map<RolePrivilageDTO>(rolePrivilage);
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
        public async Task<ActionResult<APIResponse>> UpdateRolePrivilageAsync(RolePrivilageDTO dto)
        {
            try
            {
                if (dto == null | dto.Id <= 0)
                {
                    return BadRequest();
                }
                var existingRolePrivilage = await _rolePrivilageRepository.GetAsync(rolePrivilage => rolePrivilage.Id == dto.Id, true);

                if (existingRolePrivilage == null)
                {
                    return BadRequest($"RolePrivilage not found with id: {dto.Id} to update");
                }

                var newRolePrivilage = _mapper.Map<RolePrivilage>(dto);
                await _rolePrivilageRepository.UpdateAsync(newRolePrivilage);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.Data = newRolePrivilage;
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
        [Route("Delete/{Id}", Name = "DeleteRolePrivilageById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteRolePrivilageAsync(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return BadRequest();
                }
                var rolePrivilage = await _rolePrivilageRepository.GetAsync(rolePrivilage => rolePrivilage.Id == Id);
                if (rolePrivilage == null)
                    return BadRequest($"RolePrivilage not found with id : {Id} to delete");

                await _rolePrivilageRepository.DeleteAsync(rolePrivilage);
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
