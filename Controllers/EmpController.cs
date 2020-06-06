using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using TestApi.Models;

namespace TestApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")] 
    public class EmpController : ApiController
    {
        public EmpController()
        {
        } 
        /// <summary>
        /// Returns the list of employee
        /// </summary>
        ///<returns></returns>
        [HttpGet]
        [Route("api/v1/GetEmployee")]
        public  IHttpActionResult GetEmployee()
        {
            List<Employee> lstEmployee = new List<Employee>();
            lstEmployee.Add(new Employee() {EmployeeId ="1" ,Name ="Mahesh" });
            return Ok(lstEmployee); 
        }
    }
}
