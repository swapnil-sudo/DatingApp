using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.MiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleWare(RequestDelegate next,
                                   ILogger<ExceptionMiddleWare> logger,
                                   IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context){
            try
            {
              await  _next(context);
            }
            catch(Exception ex){

                _logger.LogError(ex.ToString());
                context.Response.ContentType="application/json";
                context.Response.StatusCode=(int) HttpStatusCode.InternalServerError;

            var response=_env.IsDevelopment() 
            ?new ApiExceptions(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString())
            :new ApiExceptions(context.Response.StatusCode,"Internal Server Error");
            
            var options=new JsonSerializerOptions{PropertyNamingPolicy=JsonNamingPolicy.CamelCase};

            var json=JsonSerializer.Serialize(response,options);

            await context.Response.WriteAsync(json);
            
            }
        }
    }
}