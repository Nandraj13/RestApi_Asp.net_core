using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestApi.Models;
using RestApi.Repository;

namespace Tester
{
    public class EmployeeRepoTester
    {
        private readonly MasterContext appcontext;
        public EmployeeRepoTester()
        {
            var context = new DbContextOptionsBuilder<MasterContext>().UseInMemoryDatabase(databaseName: "master").Options;
          
            appcontext = new MasterContext(context);
        }
        [Fact]
        public void EmployeeRepo_GetById_Employee()
        {
            var emp=new EmployeeRepo(appcontext);
            var result = emp.GetById(2);
            Assert.Equal("Chirag", result.Result.Ename);
            Assert.NotNull(result);
            Assert.IsType<Employee>(result);
        }
    }
}