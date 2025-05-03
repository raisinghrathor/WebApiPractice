using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiPractice.Data;
using WebApiPractice.Models.Repositories;

namespace WebApiPractice.Filters.ActionFilters
{
    public class Shirt_ValidateShirtIdFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateShirtIdFilterAttribute(ApplicationDbContext db)
        {
            this.db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
            {
            var ShirtId = context.ActionArguments["id"] as int?;
            if (ShirtId.HasValue)
            {
                if (ShirtId.Value <= 0)
                {
                    context.ModelState.AddModelError("ShirtId", "ShirtId is invalid");
                    // context.Result = new BadRequestObjectResult(context.ModelState);
                    //or
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status=StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else// if (!ShirtRepository.ShirtExists(ShirtId.Value))
                {
                    var shirt = db.Shirts.Find(ShirtId.Value);
                    if (shirt == null) 
                    {
                        context.ModelState.AddModelError("ShirtId", "ShirtId does not exists");

                        //context.Result = new NotFoundObjectResult(context.ModelState);
                        //or
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else
                    {
                        context.HttpContext.Items["shirt"]=shirt;
                    }
                    
                }

            }
        }
    }
}
