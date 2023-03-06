
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Interfaces;
using RestApi.Models;
using System.Diagnostics;
using System.Net;

namespace RestApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
 
    public class Employee_api : Controller
    {
        private readonly MasterContext _context;
        private readonly IEmployee _employee;
        public Stopwatch sw=new Stopwatch();
        public Employee_api(MasterContext context,IEmployee employee)
        {
            _context = context;
            _employee = employee;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> Get(int? skip, int? take)
        {
            var emp = await _employee.Getall(skip, take);
            return Ok(emp);
        }
    
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
              var emp=await _employee.GetById(id);
                if(emp!=null)
                {
                return Ok(emp);
                }
                else
                {
                    return NotFound();
                }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEmployee(int id, Employee emp)
        {
            if (id != emp.Eid)
            {
                return BadRequest();
            }
            _context.Entry(emp).State=EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("Get", new { id = emp.Eid }, emp);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEmployee(Employee emp)
        {
            if (emp.Eid!=0)
            {
                return BadRequest("please dont input id...");
            }
                await _employee.CreateEmployee(emp);
                return CreatedAtAction("Get", new { id = emp.Eid }, emp);   
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id) {

            var emp = await _context.Employees.FindAsync(id);
            if(emp == null)
            {
                return NotFound();
            }
             _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();
            return emp;
        }
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchEmployee(JsonPatchDocument model,int id)
        {
            if(EmployeeExists(id))
            {
                try
                {
                    var emp = await _context.Employees.FindAsync(id);
                    model.ApplyTo(emp);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("Get", new { id = emp.Eid }, emp);
                }
                catch (JsonPatchException ex)
                {
                   
                    return NotFound(ex.Message);
                }
                catch(ArgumentNullException ex)
                {

                    return BadRequest(ex.Message);
                }
            }
            else
            { return NotFound(); }
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Eid == id);
        }
    }
}
