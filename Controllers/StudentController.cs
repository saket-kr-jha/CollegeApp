using DotNetCore_New.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {   
        [HttpGet]
        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<IEnumerable<StudentDTO>> GetStudents() {
            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            { 
                StudentId = s.StudentId,
                StudentName = s.StudentName,
                StudentPhone = s.StudentPhone,
                StudentEmail = s.StudentEmail
            });
            return Ok(students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            //BadRequest
            if(id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.FirstOrDefault(n => n.StudentId == id);

            //notfound
            if(student == null)
            {
                return NotFound();
            }
            var studentDTO = new StudentDTO
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                StudentPhone = student.StudentPhone,
                StudentEmail = student.StudentEmail
            };
            return Ok(studentDTO);
        }

        [HttpGet]
        [Route("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> GetStudenByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var student = CollegeRepository.Students.FirstOrDefault(n => n.StudentName == name);
            if (student == null)
            {
                return NotFound();
            }
            var studentDTO = new StudentDTO
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                StudentPhone = student.StudentPhone,
                StudentEmail = student.StudentEmail
            };
            return Ok(studentDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var student = CollegeRepository.Students.FirstOrDefault(n => n.StudentId == id);

            //notfound
            if (student == null)
            {
                return NotFound();
            }
            var deletedStudent = CollegeRepository.Students.FirstOrDefault(n => n.StudentId == id);
            CollegeRepository.Students.Remove(deletedStudent);
            return Ok(true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            int newId = CollegeRepository.Students.LastOrDefault().StudentId + 1;

            Student student = new Student
            {
                StudentId = newId,
                StudentName = model.StudentName,
                StudentEmail = model.StudentName,
                StudentPhone = model.StudentPhone
            };

            CollegeRepository.Students.Add(student);
            model.StudentId = student.StudentId;
            return CreatedAtRoute("GetStudentById", new { id = model.StudentId }, model);
        }
        [HttpPost]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var existingStudent = CollegeRepository.Students.FirstOrDefault(n => n.StudentId == model.StudentId);
            if (existingStudent == null)
            {
                return NotFound();
            }
            
            existingStudent.StudentName = model.StudentName;
            existingStudent.StudentPhone = model.StudentPhone;
            existingStudent.StudentEmail = model.StudentEmail;

            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}/UpdatePatch")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdatePatchStudent(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0 )
            {
                return BadRequest();
            }

            var existingStudent = CollegeRepository.Students.FirstOrDefault(n => n.StudentId == id);
            if (existingStudent == null)
            {
                return NotFound();
            }

            var studentDTO = new StudentDTO
            {
                StudentId = existingStudent.StudentId,
                StudentEmail = existingStudent.StudentEmail,
                StudentName = existingStudent.StudentName,
                StudentPhone = existingStudent.StudentPhone
            };

            patchDocument.ApplyTo(studentDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingStudent.StudentName = studentDTO.StudentName;
            existingStudent.StudentPhone = studentDTO.StudentPhone;
            existingStudent.StudentEmail = studentDTO.StudentEmail;

            return NoContent();
        }
    }
}
