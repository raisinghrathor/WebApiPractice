﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiPractice.Models;
namespace WebApiPractice.Filters.ActionFilters
{
    public class Shirt_ValidateUpdateShirtFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var id = context.ActionArguments["id"] as int?;
            var shirt = context.ActionArguments["shirt"] as Shirt;
            if (id.HasValue && shirt != null && id!=shirt.ShirtId) 
            {
                context.ModelState.AddModelError("ShirtId", "ShirtId is not same as id");
                
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
        }
    }
}
