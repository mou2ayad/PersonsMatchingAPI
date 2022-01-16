using System;

namespace Component.Utilities.ErrorHandling
{
    public class InvalidRequestException: Exception
    {
        public InvalidRequestException(): base()
        {
            this.LogAsInfo().MarkAsClientException();
        }
        public InvalidRequestException(string message) :base(message)
        {
            this.LogAsInfo().MarkAsClientException();
        }
    }
}
