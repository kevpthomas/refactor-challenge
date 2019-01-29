using System.Data;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace RefactorThis.Errors
{
    public class WebExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                HttpResponseMessage resp;

                switch (context.Exception)
                {
                    case DataException _:
                        resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        break;
                    default:
                        resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        break;
                }

                context.Result = new ErrorMessageResult(resp);
            }, cancellationToken);
        }

        public class ErrorMessageResult : IHttpActionResult
        {
            private readonly HttpResponseMessage _httpResponseMessage;
 
 
            public ErrorMessageResult(HttpResponseMessage httpResponseMessage)
            {
                _httpResponseMessage = httpResponseMessage;
            }
 
            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(_httpResponseMessage);
            }
        }
    }
}