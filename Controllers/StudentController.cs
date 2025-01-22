using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Data.Repository;
using DotNetCore_New.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "CorsPolicy")]
    //[Authorize(Roles = "Admin")]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        //private readonly ICollegeRepository<Student> _studentRepository;
        private readonly IStudentRepository _studentRepository;
        private APIResponse _apiResponse;

        public StudentController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _apiResponse = new();
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync() {
            try
            {
                var students = await _studentRepository.GetAllAsync();
                //var students = await _dbContext.Students.Select(s => new StudentDTO()
                //{
                //    StudentId = s.StudentId,
                //    StudentEmail = s.StudentEmail,
                //    StudentName = s.StudentName,
                //    StudentPhone = s.StudentPhone,
                //    DOB = s.DOB
                //}).ToListAsync();
                _apiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
           
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            try
            {  //BadRequest
                if (id <= 0)
                {
                    return BadRequest();
                }
                var student = await _studentRepository.GetAsync(student => student.StudentId == id);

                //notfound
                if (student == null)
                {
                    return NotFound();
                }
                _apiResponse.Data = _mapper.Map<StudentDTO>(student);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        } 
        [HttpGet]
        [Route("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudenByNameAsync(string name)
        {
            try {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetAsync(student => student.StudentName.ToLower().Contains(name));
                if (student == null)
                {
                    return NotFound();
                }
                _apiResponse.Data = _mapper.Map<StudentDTO>(student);
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
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
            
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteStudentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }
                var student = await _studentRepository.GetAsync(student => student.StudentId == id);

                //notfound
                if (student == null)
                {
                    return NotFound($"The student with id {id} not found");
                }
                await _studentRepository.DeleteAsync(student);
                _apiResponse.Data = true;
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

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody]StudentDTO model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                Student student = _mapper.Map<Student>(model);


                var newStudent = await _studentRepository.CreateAsync(student);
                model.StudentId = newStudent.StudentId;
                _apiResponse.Data = model;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetStudentById", new { id = model.StudentId }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
           
        }
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            try {
                if (model == null)
                {
                    return BadRequest();
                }

                var existingStudent = await _studentRepository.GetAsync(student => student.StudentId == model.StudentId, true);
                if (existingStudent == null)
                {
                    return NotFound();
                }

                var newRecord = _mapper.Map<Student>(model);

                await _studentRepository.UpdateAsync(newRecord);
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
            
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePatch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePatchStudentAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            try {
                if (patchDocument == null || id <= 0)
                {
                    return BadRequest();
                }

                var existingStudent = await _studentRepository.GetAsync(student => student.StudentId == id, true);
                if (existingStudent == null)
                {
                    return NotFound();
                }

                var studentDTO = _mapper.Map<StudentDTO>(existingStudent);

                patchDocument.ApplyTo(studentDTO, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                existingStudent = _mapper.Map<Student>(studentDTO);
                await _studentRepository.UpdateAsync(existingStudent);
                return NoContent();
            }
            catch (Exception ex)
            {
                _apiResponse.Data = null;
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
            
        }
    }
}
