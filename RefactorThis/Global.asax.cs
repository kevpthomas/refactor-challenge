using System.Web.Http;
using RefactorThis.IoC;
using TinyIoC;

namespace RefactorThis
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AutoMapperConfig.ConfigureMappings();

            ConfigureTinyIocContainer();
        }

        private void ConfigureTinyIocContainer()
        {
            // auto-register all dependencies
            TinyIoCContainer.Current.AutoRegister();

            // Set Web API dependency resolver to allow injection into API controllers
            GlobalConfiguration.Configuration.DependencyResolver = new TinyIocWebApiDependencyResolver(TinyIoCContainer.Current);
        }
    }
}
