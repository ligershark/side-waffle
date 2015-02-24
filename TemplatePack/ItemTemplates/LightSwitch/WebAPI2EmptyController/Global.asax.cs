
namespace $rootnamespace$
{ 
    using System.Web.Routing;
    using System.Web.Http;
 
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );  
        }
    }
}