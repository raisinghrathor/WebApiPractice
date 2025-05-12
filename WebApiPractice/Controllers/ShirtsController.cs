 using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using WebApiPractice.Attributes;
using WebApiPractice.Data;
using WebApiPractice.Filters.ActionFilters;
using WebApiPractice.Filters.AuthFilter;
using WebApiPractice.Filters.ExceptionFilter;
using WebApiPractice.Models;
using WebApiPractice.Models.Repositories;

namespace WebApiPractice.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [JwtTokenAuthFilterAttribute]
    public class ShirtsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ShirtsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        //[Route("/shirts")]
        [RequiredClaim("read","true")]
        public IActionResult GetShirts()
        {
            return Ok(dbContext.Shirts.ToList());
           // return Ok(ShirtRepository.GetShirts()) ;
        }
        [HttpGet("{id}")]
        // [Route("/shirts/{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("read", "true")]
        public IActionResult GetShirtById(int id)
        {
            //if (id <= 0)
            //    return BadRequest();
            //var shirt = ShirtRepository.GetShirtById(id); //.FirstOrDefault(s => s.ShirtId == id);
            //if (shirt == null)
            //    return NotFound();
            //return Ok(shirt);
          // var shirt= HttpContext.Items["shirt"] as Shirt;

            // return Ok(ShirtRepository.GetShirtById(id));
            //or
            return Ok(HttpContext.Items["shirt"]);
        }
        [HttpPost]
        //[Route("/shirts")]
        [TypeFilter(typeof(Shirt_ValidateCreateShirtFilter))]
        [RequiredClaim("write", "true")]
        public IActionResult CreateShirt([FromBody] Shirt shirt)
        {
           
           // ShirtRepository.CreateShirt(shirt);
           this.dbContext.Shirts.Add(shirt);
            this.dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetShirtById),new { id = shirt.ShirtId },shirt);
        }
        [HttpPut("{id}")]
        // [Route("/shirts/{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [Shirt_ValidateUpdateShirtFilter]
        [TypeFilter(typeof(Shirt_HandleShirtUpdateExceptionFilter))]
        [RequiredClaim("write", "true")]
        public IActionResult UpdateShirt(int id,Shirt shirt)
        {
           var ExistingShirt = HttpContext.Items["shirt"] as Shirt;
            ExistingShirt.Brand = shirt.Brand;
            ExistingShirt.Price = shirt.Price;
            ExistingShirt.Gender = shirt.Gender;
            ExistingShirt.Color = shirt.Color;
            ExistingShirt.Size = shirt.Size;
            dbContext.SaveChanges();

            //ShirtRepository.UpdateShirt(ExistingShirt);
           
            return NoContent();
        }
        [HttpDelete("{id}")]
        //[Route("/shirts/{id}")]
        [TypeFilter(typeof(Shirt_ValidateShirtIdFilterAttribute))]
        [RequiredClaim("delete", "true")]
        public IActionResult DeleteShirt(int id)
        {
            var shirtToDelete = HttpContext.Items["shirt"] as Shirt;
            dbContext.Remove(shirtToDelete);
            dbContext.SaveChanges();
           
            return Ok(shirtToDelete);
            
        }
    }
}
