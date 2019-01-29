using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using RefactorThis.Core.Interfaces;
using RefactorThis.Errors;
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

            // Configure global exception logger
            GlobalConfiguration
                .Configuration
                .Services
                .Add(typeof(IExceptionLogger), new WebExceptionLogger(TinyIoCContainer.Current.Resolve<ILogger>()));

            GlobalConfiguration
                .Configuration
                .Services
                .Replace(typeof(IExceptionHandler), new WebExceptionHandler());
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
