using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApi.Interfaces;
using RestApi.Models;

namespace RestApi.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly MasterContext _context;
        public EmployeeRepo(MasterContext context)
        {
            _context = context;
        }

        public async Task<Employee> CreateEmployee(Employee emp)
        {
                _context.Add(emp);
                await _context.SaveChangesAsync();
                return emp;
        }

        public async Task<IEnumerable<Employee>> Getall(int? skip,int? take)
        {
            var emp = _context.Employees.AsQueryable();


            emp = _context.Employees;

            if (skip != null)
            {
                emp = emp.Skip((int)skip);
            }

            if (take != null)
            {
                emp = emp.Take((int)take);
            }

            return await emp.ToListAsync();
        }

        public async Task<Employee> GetById(int id)
        {
            var emp= await _context.Employees.FindAsync(id);
            return emp;
        }
    }
}
