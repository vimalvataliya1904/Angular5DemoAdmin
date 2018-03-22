using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminAPI
{
    public static class APIResponse
    {
        public static object ResponseValidation(this ModelStateDictionary ModelState)
        {
            return new
            {
                Status = APIResponseStatus.NotValidate,
                Message = "Validation Failed",
                OpStatus = APIOpStatus.ValidationFailed,
                Data = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(x => x.ErrorMessage)
                //result = false,
                //message = "validation",
                //error = ModelState.Values.SelectMany(v => v.Errors).ToList().Select(x => x.ErrorMessage)
            };
        }
        
        public static object SetResponse(APIResponseStatus ResponseStatus, string Message, APIOpStatus OperationStatus, object Data)
        {
            return new
            {
                ResponseStatus,
                Message,
                OperationStatus,
                Data
            };
        }
    }


    public enum APIResponseStatus
    {
        Ok, Error, NotFound, NotValidate
    }

    public enum APIOpStatus
    {
        Success, Failed, Error, Warning, Authorize, ValidationFailed
    }
}
