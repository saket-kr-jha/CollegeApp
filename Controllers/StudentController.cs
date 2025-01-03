using AutoMapper;
using DotNetCore_New.Data;
using DotNetCore_New.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly CollegeDBContext _dbContext;
        private readonly IMapper _mapper;

        public StudentController(CollegeDBContext dbContext, IMapper mapper)
        {
                _dbContext = dbContext;
                _mapper = mapper; 
        }

        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync() {
            var students = await _dbContext.Students.ToListAsync();
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
            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentId == id);

            //notfound
            if(student == null)
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

            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentName == name);
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
            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentId == id);

            //notfound
            if (student == null)
            {
                return NotFound();
            }
            var deletedStudent = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentId == id);
            _dbContext.Students.Remove(deletedStudent);
            await _dbContext.SaveChangesAsync();
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

            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            model.StudentId = student.StudentId;
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

            var existingStudent = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentId == model.StudentId);
            if (existingStudent == null)
            {
                return NotFound();
            }

            existingStudent = _mapper.Map<Student>(model);
            
            //existingStudent.StudentName = model.StudentName;
            //existingStudent.StudentPhone = model.StudentPhone;
            //existingStudent.StudentEmail = model.StudentEmail;
            //existingStudent.DOB = model.DOB;
            _dbContext.SaveChangesAsync();
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

            var existingStudent = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentId == id);
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
            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.StudentPhone = studentDTO.StudentPhone;
            existingStudent.StudentEmail = studentDTO.StudentEmail;
            existingStudent.DOB = studentDTO.DOB;

            _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
