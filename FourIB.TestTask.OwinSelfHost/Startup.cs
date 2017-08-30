using Owin;
using System.Web.Http;

namespace FourIB.TestTask.OwinSelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            //config.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());
            app.UseWebApi(config);
        }
    }

    // Альтернативный метод подключения API
    //internal class AssembliesResolver : DefaultAssembliesResolver
    //{
    //    public override ICollection<Assembly> GetAssemblies()
    //    {
    //        ICollection<Assembly> assemblies = base.GetAssemblies();
    //        var apiAssembly = Assembly.LoadFrom(@"FourIB.TestTask.WebApi.dll");
    //        assemblies.Add(apiAssembly);
    //        return assemblies;
    //    }
    //}
}