using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using RefactorThis.Core.Interfaces;

namespace RefactorThis
{
    public class WebExceptionLogger : IExceptionLogger
    {
        private readonly ILogger _logger;

        public WebExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() => _logger.Log(context.ExceptionContext.Exception), cancellationToken);
        }
    }
}