using Microsoft.AspNetCore.Mvc.Filters;
using WebApiPractice.Models.Repositories;
using WebApiPractice.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Reflection;
using WebApiPractice.Data;

namespace WebApiPractice.Filters.ActionFilters
{
    public class Shirt_ValidateCreateShirtFilter : ActionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_ValidateCreateShirtFilter(ApplicationDbContext db)
        {
            this.db = db;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var shirt = context.ActionArguments["shirt"] as Shirt;
            if (shirt == null)
            {
                context.ModelState.AddModelError("Shirt", "Shirt object is null");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else
            {

              var ShirtExists=  db.Shirts.FirstOrDefault(x => !string.IsNullOrWhiteSpace(shirt.Brand) &&
                                            !string.IsNullOrWhiteSpace(x.Brand) &&
                                    x.Brand.ToLower()==shirt.Brand&&
                                    !string.IsNullOrWhiteSpace(shirt.Gender) &&
                                    !string.IsNullOrWhiteSpace(x.Gender) &&
                                    x.Gender.ToLower()==shirt.Gender &&
                                    !string.IsNullOrWhiteSpace(shirt.Color) &&
                                    !string.IsNullOrWhiteSpace(x.Color) &&
                                    x.Color.ToLower()==shirt.Color &&
                                    shirt.Size.HasValue &&
                                    x.Size.HasValue &&
                                    shirt.Size.Value == x.Size.Value);
               // var ShirtExists = ShirtRepository.GetShirtByProperties(shirt.Brand, shirt.Gender, shirt.Color, shirt.Size);
                if (ShirtExists != null)
                {
                    context.ModelState.AddModelError("Shirt", "Shirt already exists");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }



        }
    }
}
