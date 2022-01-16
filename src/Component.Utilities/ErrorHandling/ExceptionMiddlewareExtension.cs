using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Component.Utilities.ErrorHandling
{
    public static class ExceptionMiddlewareExtention
    {

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder app, ILogger log, string appName, bool isDevelopment)
        {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {

                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        Exception exception = contextFeature.Error;
                        if (contextFeature.Error is AggregateException)
                            exception = exception.InnerException;
                        HttpExceptionDetails errorDetails;
                        if (exception.IsClientException())
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            errorDetails = new HttpExceptionDetails()
                            {
                                StatusCode = (int)HttpStatusCode.BadRequest,
                                ErrorMessage = exception.Message
                            };
                        }
                        else
                        {

                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            errorDetails = new HttpExceptionDetails()
                            {
                                StatusCode = (int)HttpStatusCode.InternalServerError,
                                ErrorMessage = "Internal Server Error"
                            };

                        }

                        if (isDevelopment && !exception.IsClientException())
                            errorDetails.ErrorMessage = exception.ToString();

                        if (exception.IsWithNoLog())
                            errorDetails.WithNolog();
                        else
                        {
                            var parameters = JsonConvert.SerializeObject(context.Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString()));
                            if (exception.IsLoggedAsInfo())
                            {
                                log.LogInformation(exception, "ErorrCode:{0} Service:{1} Path:{2} Parameters:{3}\n Exception=> ", errorDetails.ErrorCode, appName, context.Request.Path.Value, parameters);
                            }
                            else
                            {
                                log.LogError(exception, "ErorrCode:{0} Service:{1} Path:{2} Parameters:{3}\n Exception=> ", errorDetails.ErrorCode, appName, context.Request.Path.Value, parameters);

                            }

                        }
                        await context.Response.WriteAsync(errorDetails.ToString());
                    }
                });
            });
            return app;
        }
    }
}
