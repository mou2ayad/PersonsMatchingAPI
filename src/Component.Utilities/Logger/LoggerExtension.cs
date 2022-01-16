using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Component.Utilities.Logger
{
    public static class LoggerExtension
    {
        public static void LogErrorDetails(this ILogger logger, Exception ex, params object[] parameters)
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            var parametersJson = parameters == null ? "" : JsonConvert.SerializeObject(parameters.Where(e => e != null).ToArray());
            logger.LogError(ex, "Error Code:{0} Method:{1}.{2} Parameters:{3}\n Exception=>",
                Guid.NewGuid(), method.Name, method.ReflectedType.Name, parametersJson);
        }
        public static void LogInfoDetails(this ILogger logger, string msg, params object[] list)
        {
            StackTrace stackTrace = new StackTrace();
            MethodBase method = stackTrace.GetFrame(1).GetMethod();
            logger.LogInformation("From Method:{0}.{1}, Message: {2}",
                 method.Name, method.ReflectedType.Name, string.Format(msg, list));
        }
    }
}
