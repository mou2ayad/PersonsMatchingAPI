using System;

namespace Component.Utilities.ErrorHandling
{
    public static class ClientExceptionExtension
    {
        public static Exception MarkAsClientException(this Exception exception)
        {
            if(!exception.Data.Contains("ClientException"))
                exception.Data.Add("ClientException", true);
            return exception;
        }

        public static bool IsClientException(this Exception exception) => exception.Data.Contains("ClientException");

        public static bool IsWithNoLog(this Exception exception) => exception.Data.Contains("WithNoLog");
        
        public static Exception LogAsInfo(this Exception exception)
        {
            if (!exception.Data.Contains("AsInfo"))
                exception.Data.Add("AsInfo", true);
            return exception;
        }
        public static bool IsLoggedAsInfo(this Exception exception) => exception.Data.Contains("AsInfo");
             
    }
}
