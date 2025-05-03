using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiPractice.Data;
using WebApiPractice.Models;
using WebApiPractice.Models.Repositories;

namespace WebApiPractice.Filters.ExceptionFilter
{
    public class Shirt_HandleShirtUpdateExceptionFilter:ExceptionFilterAttribute
    {
        private readonly ApplicationDbContext db;

        public Shirt_HandleShirtUpdateExceptionFilter(ApplicationDbContext db)
        {
            this.db = db;
        }
        public override void OnException(ExceptionContext context)
        {
          
            var strshirtId = context.RouteData.Values["id"] as string;
            if (int.TryParse(strshirtId, out int shirtId)) 
            {
                if(db.Shirts.FirstOrDefault(x=>x.ShirtId==shirtId)==null)
                {
                    context.ModelState.AddModelError("ShirtId","Shirt does not exists anymore");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status404NotFound
                    };
                    context.Result = new NotFoundObjectResult(problemDetails);
                }
            }
        }
    }
}
