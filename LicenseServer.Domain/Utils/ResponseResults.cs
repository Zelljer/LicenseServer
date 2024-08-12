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
            return new OkObjectResult(HttpResults.StringResult.Fails(errors));
        }

        public static ActionResult ErrorOkResult(string error)
        {
            return ErrorsOkResult(new List<string> { error });
        }

        public static ActionResult ErrorsBadResults(List<string> errors, ILogger logger, Exception exception)
        {
            return new BadRequestObjectResult(HttpResults.StringResult.Fails(errors));
        }

        public static ActionResult ErrorBadResult(string error, ILogger logger, Exception exception)
        {
            return new BadRequestObjectResult(HttpResults.StringResult.Fail(error));
        }
    }
}