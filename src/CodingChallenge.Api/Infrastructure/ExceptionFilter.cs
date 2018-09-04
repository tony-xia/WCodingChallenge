using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CodingChallenge.Api.Infrastructure
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _env;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IHostingEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            if (_env.IsDevelopment())
            {
                context.Result = new JsonResult(new { Message = context.Exception.Message, Exception = context.Exception })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            else
            {
                context.Result = new JsonResult(new { Message = "Unhandled exception" })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
