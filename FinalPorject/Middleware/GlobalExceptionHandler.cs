using Data;
using Microsoft.AspNetCore.Diagnostics;
using Utils;

namespace Middlewares
{
    public static class GlobalExceptionHandler
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

                    if (ex is AppException appEx)
                    {
                        context.Response.StatusCode = appEx.ExceptionResult.StatusCode;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(appEx.ExceptionResult.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(new StandardResult
                        {
                            StatusCode = 500,
                            Success = false,
                            Messages = new List<string> { "Server Error" }
                        }.ToString());
                    }

                });
            });
        }
    }
}