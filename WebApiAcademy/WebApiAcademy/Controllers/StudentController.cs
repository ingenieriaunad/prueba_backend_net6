using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiAcademy.DTOs;
using WebApiAcademy.Models;
using WebApiAcademy.Services;
using WebApiAcademy.Validations;

namespace WebApiAcademy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService _studentService;
        private readonly IPersonService _personService;
        private readonly IRolService _rolService;
        private readonly IUserService _userService;
        public StudentController(IStudentService studentService, IPersonService personService, IRolService rolService, IUserService userService)
        {
            _studentService = studentService;
            _personService = personService;
            _rolService = rolService;
            _userService = userService;

        }

        [HttpGet]
        public async Task<ActionResult>  GetStudents([FromQuery] Pagination pagination)
        {
            var students = await _studentService.GetPaginated(pagination);
            return Ok(students);
        }
        [HttpPost]
        public async Task<ActionResult> CreateStudent([FromForm] StudentInformation data)
        {
            try
            {
                if(data == null) return BadRequest();
                if(string.IsNullOrEmpty(data.Name) )
                {
                    return BadRequest(new { message = "Nombre es requerido" });
                }
                if(string.IsNullOrEmpty(data.LastName) )
                {
                    return BadRequest(new { message = "Apellido es requerido" });
                }
                if(string.IsNullOrEmpty(data.Email) )
                {
                    return BadRequest(new { message = "Email es requerido" });
                }
                if(!UserValidation.IsValidEmail(data.Email))
                {
                    return BadRequest(new { message = "Email no válido" });
                }
                if(string.IsNullOrEmpty(data.CardId))
                {
                    return BadRequest(new { message = "Cédula es requerida" });
                }
                if(string.IsNullOrEmpty(data.Password))
                {
                    return BadRequest(new { message = "Contraseña es requerida" });
                }
                if(!UserValidation.IsValidPassword(data.Password))
                {
                    return BadRequest(new { message = "Contraseña no válida" });
                }
                if(string.IsNullOrEmpty(data.Phone))
                {
                    return BadRequest(new { message = "Teléfono es requerido" });
                }
                Person person = new()
                {
                    Id = Guid.NewGuid(),
                    Name = data.Name,
                    LastName = data.LastName,
                    CardId = data.CardId,
                    Phone = data.Phone,
                };
                Person personExist = await _personService.GetByCardId(data.CardId);
                if(personExist != null) return BadRequest(new { message = "Documento ya existe" });
                Rol rol= await _rolService.GetByName("Estudiante");
                User user = new()
                {
                    Id       = Guid.NewGuid(),
                    Email    = data.Email,
                    Password = AuthService.HashPassword(data.Password),
                    PersonId = person.Id,
                    RolId    = rol.Id
                };
                User userExist = await _userService.GetByEmail(data.Email);
                if(userExist != null) return BadRequest(new { message = "Email ya existe" });
                await _personService.Save(person);
                await _userService.Save(user);
                return Ok(new { message = "Estudiante creado correctamente" });
            }
            catch (Exception e)
            {
                
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost("edit/{userId:guid}")]
        public async Task<ActionResult> EditStudent(Guid userId,[FromForm] StudentInformation data)
        {
            try
            {
                if(data == null) return BadRequest();
                if(string.IsNullOrEmpty(data.Name) )
                {
                    return BadRequest(new { message = "Nombre es requerido" });
                }
                if(string.IsNullOrEmpty(data.LastName) )
                {
                    return BadRequest(new { message = "Apellido es requerido" });
                }
                if(string.IsNullOrEmpty(data.Email) )
                {
                    return BadRequest(new { message = "Email es requerido" });
                }
                if(!UserValidation.IsValidEmail(data.Email))
                {
                    return BadRequest(new { message = "Email no válido" });
                }
                if(string.IsNullOrEmpty(data.CardId))
                {
                    return BadRequest(new { message = "Cédula es requerida" });
                }
                if(string.IsNullOrEmpty(data.Phone))
                {
                    return BadRequest(new { message = "Teléfono es requerido" });
                }
                if(string.IsNullOrEmpty(userId.ToString()))
                {
                    return BadRequest(new { message = "Id de usuario es requerido" });
                }
                User user =await _userService.GetById(userId)!;
                if(user == null) return NotFound(new{ message = "No se encontró el usuario" });
                User newUser=new()
                {
                    Id = user.Id,
                    Email = data.Email,
                    PersonId = user.PersonId,
                    RolId = user.RolId
                };
                if(!string.IsNullOrEmpty(data.Password))
                {
                    if(!UserValidation.IsValidPassword(data.Password))
                    {
                        return BadRequest(new { message = "Contraseña no válida" });
                    }
                    newUser.Password = AuthService.HashPassword(data.Password);
                }
                Person person = new()
                {
                    Id = user.PersonId,
                    Name = data.Name,
                    LastName = data.LastName,
                    CardId = data.CardId,
                    Phone = data.Phone,
                };
                await _userService.Update(newUser);
                await _personService.Update(person);
                return Ok(new { message = "Estudiante actualizado correctamente" });
            }
            catch (Exception e)
            {
                
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try{
                User user= await _userService.GetById(id)!;
                 await _userService.Delete(id);
                 await _personService.Delete(user.PersonId);
                return Ok(new { message = "Estudiante eliminado correctamente" });
            }
            catch (Exception e)
            {
                
                return BadRequest(new { message = e.Message });
            }
        }

    }
}
