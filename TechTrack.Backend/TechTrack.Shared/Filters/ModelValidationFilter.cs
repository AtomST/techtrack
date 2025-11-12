using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TechTrack.Shared.Filters
{
    public class ModelValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new
                    {
                        Field = x.Key,
                        Message = x.Value.Errors.First().ErrorMessage
                    });

                context.Result = new BadRequestObjectResult(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "ValidationError",
                    Errors = errors
                });
            }
        }
    }
}
