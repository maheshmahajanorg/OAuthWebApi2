using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TestApi.App_Start;

namespace TestApi.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-My-Header")]
    public class TestAuthController : ApiController
    { 


        [HttpGet] 
        [AllowAnonymous]
        [Route("api/testauth/get")]
        public IHttpActionResult  Get()
        {
            return Ok("");
        } 


        [HttpGet]
        [Route("api/testauth/user")] 
        [Authorize(Roles ="user")]
        public IHttpActionResult UserAuth()
        {
            return Ok("Users api");
        }


        [HttpGet]
        [Route("api/testauth/admin")]
        [Authorize(Roles = "admin")]
        public IHttpActionResult AdminAuth()
        {
            return Ok("Users api");
        }

        [HttpGet]
        [Route("api/testauth/userfeature")]
        [CustomAuthorize(FeatureId = "001")]
        public IHttpActionResult UserFeature()
        {
            return Ok("Users api");
        }

        [HttpGet]
        [Route("api/testauth/adminfeature")]
        [CustomAuthorize(FeatureId = "002")]
        public IHttpActionResult AdminFeature()
        {
            return Ok("Users api");
        }

    }
}
