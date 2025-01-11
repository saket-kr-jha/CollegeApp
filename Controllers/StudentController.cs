using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Data.Repository;
using DotNetCore_New.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;

        public StudentController(ICollegeRepository<Student> studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
                _mapper = mapper; 
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync() {
            var students = await _studentRepository.GetAllAsync();
            //var students = await _dbContext.Students.Select(s => new StudentDTO()
            //{
            //    StudentId = s.StudentId,
            //    StudentEmail = s.StudentEmail,
            //    StudentName = s.StudentName,
            //    StudentPhone = s.StudentPhone,
            //    DOB = s.DOB
            //}).ToListAsync();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);
            return Ok(studentDTOData);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            //BadRequest
            if(id <= 0)
            {
                return BadRequest();
            }
            var student = await _studentRepository.GetByIdAsync(student => student.StudentId == id);

            //notfound
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> GetStudenByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var student = await _studentRepository.GetByNameAsync(student => student.StudentName.ToLower().Contains(name)); 
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = await _studentRepository.GetByIdAsync(student => student.StudentId == id);

            //notfound
            if (student == null)
            {
                return NotFound($"The student with id {id} not found");
            }
            await _studentRepository.DeleteAsync(student);
            return Ok(true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody]StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            Student student = _mapper.Map<Student>(model);

            var newStudent = await _studentRepository.CreateAsync(student);
            model.StudentId = newStudent.StudentId;
            return CreatedAtRoute("GetStudentById", new { id = model.StudentId }, model);
        }
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.StudentId == model.StudentId, true);
            if (existingStudent == null)
            {
                return NotFound();
            }

            var newRecord = _mapper.Map<Student>(model);

            await _studentRepository.UpdateAsync(newRecord);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePatch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdatePatchStudentAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0 )
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetByIdAsync(student => student.StudentId == id, true);
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
    }
}
