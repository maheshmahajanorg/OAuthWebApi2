using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
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



        /// <summary>
        /// Returns the list of employee
        /// </summary>
        ///<returns></returns>
        [HttpGet]
        [Route("api/v1/ConnectVM")]
        public IHttpActionResult GetVmData()
        {
            HttpWebRequest webRequest =(HttpWebRequest) HttpWebRequest.Create("http://10.0.0.4/test.txt");
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            string text = streamReader.ReadToEnd();
            return Ok(text);
        }
    }
}
