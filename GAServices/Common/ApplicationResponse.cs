using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GAServices.Common
{
    public class AppResponse
    {
        public string Message;
        public ResponseStatus Status;
        public Dictionary<object, object> ReturnData;

        public AppResponse()
        {
            ReturnData = new Dictionary<object, object>();
            Status = ResponseStatus.FAILURE;
        }

        public AppResponse(ResponseStatus _status)
        {
            ReturnData = new Dictionary<object, object>();
            Status = _status;
        }

        public AppResponse(ResponseStatus _status, string _Message)
        {
            Status = _status;
            Message = _Message;
            ReturnData = new Dictionary<object, object>();
        }

    }

    public enum ResponseStatus
    {
        SUCCESS,
        FAILURE,
        ERROR,
        EXCEPTION,
        INVALID_DATA,
        DUPLICATE_DATA,
        MISSING_DATA,
        WARNING,
        INFORMATION,
        CRITICAL
    }
}
