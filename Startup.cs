using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using TestApi.Auth;
[assembly: OwinStartup(typeof(TestApi.Startup))]
namespace TestApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Allow Cors option 
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var authProvider  = new CustomAuthorizationProvider();

            OAuthAuthorizationServerOptions option = new OAuthAuthorizationServerOptions();
            string tokenServer = ConfigurationManager.AppSettings["LocalTokenServer"].ToString();

            option.TokenEndpointPath = new PathString("/token");
            //option.TokenEndpointPath = new PathString(tokenServer);
            option.AllowInsecureHttp = true;
            option.AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(60);
            option.Provider = authProvider;
            
            //Refresh Token Provider 
            option.RefreshTokenProvider = new RefreshTokenProvider();
            
            app.UseOAuthAuthorizationServer(option); 
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration); 

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
        }
    }
}
