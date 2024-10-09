using ConferenceAPI.Data;
using ConferenceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentsController : ControllerBase
    {
        public DanaBazaDeDateContext _context;

        public DepartamentsController(DanaBazaDeDateContext context)
        {
            _context = context;
        }
        [HttpGet("get-departaments")]
        public ActionResult GetAllDepartaments()
        {
            List<Departaments> departaments = _context.Departaments.ToList();
            return Ok(departaments);
        }

        [HttpGet("get-departament/{id}")]
        public ActionResult GetDepartament(int id)
        {
            var department = _context.Departaments.Find(id); 
            if (department == null)
            {
                return NotFound("Department not found"); 
            }
            return Ok(department); 
        }

        [HttpPost("post-departament")]
        public ActionResult CreateDepartament([FromBody] DepartamentsRequest departamentsRequest)
        {
            if (departamentsRequest == null)
            {
                return BadRequest("Input data");
            }

            var existingDepartament = _context.Departaments.FirstOrDefault(dep => dep.Code == departamentsRequest.Code);
            if (existingDepartament != null)
            {
                return Conflict("Departament already exists");
            }

            if (departamentsRequest.Name == null)
            {
                return BadRequest("Input Name");
            }

            if (departamentsRequest.Code == null)
            {
                return BadRequest("Input Code");
            }

            if(departamentsRequest.Description == null)
            {
                return BadRequest("Input Description");
            }

            if (string.IsNullOrWhiteSpace(departamentsRequest.Employees.ToString()))
            {
                return BadRequest("Input number of employees for departament");
            }

            Departaments departament = new Departaments
            {
                Name = departamentsRequest.Name,
                Code = departamentsRequest.Code,
                Description = departamentsRequest.Description,
                Employees = departamentsRequest.Employees
            };

            _context.Departaments.Add(departament);
            _context.SaveChanges();
            return Ok("Departament created");
        }

        [HttpPut("put-departament/{id}")]
        public ActionResult UpdateDepartament(int id, [FromBody] DepartamentsRequest departamentsRequest)
        {
            var existingDepartament = _context.Departaments.FirstOrDefault(dep => dep.Id == id);
            if (departamentsRequest == null)
            {
                return BadRequest("");
            }

            if (departamentsRequest.Name == null)
            {
                //departamentsRequest.Name = existingDepartament.Name;
                return BadRequest("Input Name");
            }

            if (departamentsRequest.Code == null)
            {
                //departamentsRequest.Code = existingDepartament.Code;
                return BadRequest("Input Code");
            }

            if (departamentsRequest.Description == null)
            {
                //departamentsRequest.Description = existingDepartament.Description;
                return BadRequest("Input Description");
            }

            if (string.IsNullOrWhiteSpace(departamentsRequest.Employees.ToString()))
            {
                //departamentsRequest.Employees= existingDepartament.Employees;
                return BadRequest("Input number of employees for departament");
            }

            if (existingDepartament == null)
            {
                return NotFound("Departament not found");
            }

            existingDepartament.Name = departamentsRequest.Name;
            existingDepartament.Code = departamentsRequest.Code;
            existingDepartament.Description = departamentsRequest.Description;
            existingDepartament.Employees = departamentsRequest.Employees;
            _context.SaveChanges();
            return Ok("Update succesfull");
        }

        [HttpDelete("delete-departament/{id}")]
        public IActionResult DeleteDepartament(int id)
        {
            var foundDepartament = _context.Departaments.FirstOrDefault(i => i.Id == id);
            if (foundDepartament == null)
            {
                return NotFound("Departament not found");
            }
            _context.Departaments.Remove(foundDepartament);
            _context.SaveChanges();
            return Ok("Departament Deleted");
        }

    }
}
