using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Domain.Utils
{
    public static class ResponseResults
    {
        public static ActionResult SuccessResult<T>(T data)
        {
            return new OkObjectResult(new HTTPResult<T> { IsSuccsess = true, Data = data });
        }

        public static ActionResult ErrorsOkResult(List<string> errors)
        {
            return new OkObjectResult(new HTTPResult<string> { IsSuccsess = false, Errors = errors });
        }

        public static ActionResult ErrorOkResult(string error)
        {
            return ErrorsOkResult(new List<string> { error });
        }

        public static ActionResult ErrorsBadResult(List<string> errors)
        {
            return new OkObjectResult(new HTTPResult<string> { IsSuccsess = false, Errors = errors });
        }

        public static ActionResult ErrorBadResult(string error)
        {
            return ErrorsBadResult(new List<string> { error });
        }



    }
}
