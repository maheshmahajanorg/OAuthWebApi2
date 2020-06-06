using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace TestApi.App_Start
{

    /// <summary>
    ///  custome authorize based on cliam value in feature
    /// </summary>
    public class CustomAuthorize :   ActionFilterAttribute
    {
        public string FeatureId { get; set; }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                ClaimsIdentity identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                if(identity.HasClaim(x => x.Type == "Feature"))
                {
                    string  featureId  =   identity.Claims.Single(x => x.Type == "Feature").Value; 
                    if(this.FeatureId == featureId)
                    {
                        base.OnActionExecuting(actionContext);
                    } 
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    }
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}