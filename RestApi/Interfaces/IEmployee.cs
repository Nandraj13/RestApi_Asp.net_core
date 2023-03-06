using Microsoft.AspNetCore.Mvc;
using RestApi.Models;

namespace RestApi.Interfaces
{
    public interface IEmployee
    {
        public Task<IEnumerable<Employee>> Getall(int? skip,int?take);
        public Task<Employee> GetById(int id);
        public Task<Employee> CreateEmployee(Employee emp);
    }
}
