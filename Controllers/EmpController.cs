using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
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
        public async Task<IHttpActionResult> GetVmData()
        {
            HttpWebRequest webRequest =(HttpWebRequest) HttpWebRequest.Create("http://10.0.0.4/test.txt");
            WebResponse webResponse = webRequest.GetResponse();
            Stream stream = webResponse.GetResponseStream();
            await uploadblob(stream);
            StreamReader streamReader = new StreamReader(stream);
            string text = streamReader.ReadToEnd();
            return Ok(text);
        }

        private static async Task uploadblob(Stream Stream)
        {
            string connstring = "DefaultEndpointsProtocol=https;AccountName=storageaccountfreer950a;AccountKey=oTz/hlDMRm/6imbu/DibwJDGc5XV6hiElMoeC0bVJp73U/9uMjNW/kzOA0SaEM2aQhMnx3uCUHa8P6vDs5h+JQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount l_account;
            if (CloudStorageAccount.TryParse(connstring, out l_account))
            {
                CloudBlobClient l_client = l_account.CreateCloudBlobClient();
                CloudBlobContainer l_container =
                l_client.GetContainerReference("staging");
                CloudBlockBlob l_blob = l_container.GetBlockBlobReference("test.txt");
                await l_blob.UploadFromStreamAsync(Stream);
            }
        }
    }
}
