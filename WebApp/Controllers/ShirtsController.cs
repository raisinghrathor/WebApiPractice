using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class ShirtsController : Controller
    {
        private readonly IWebApiExecuter apiExecuter;

        public ShirtsController(IWebApiExecuter apiExecuter)
        {
            this.apiExecuter = apiExecuter;
        }
        public async Task<IActionResult> Index()
        {


            return View(await apiExecuter.InvokeGet<List<Shirt>>("Shirts"));
        }
        public IActionResult CreateShirt()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateShirt(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await apiExecuter.InvokePost("Shirts", shirt);
                    if (response != null)
                    {
                        return RedirectToAction("Index");

                    }
                }
                catch (WebApiException ex)
                {

                    HandleWebApiException(ex);
                }

            }
            return View();
        }
        public async Task<IActionResult> UpdateShirt(int ShirtId)
        {
            var id = ShirtId;
            try
            {
                var shirt = await apiExecuter.InvokeGet<Shirt>($"Shirts/{id}");
                if (shirt != null)
                {
                    return View(shirt);
                }
            }
            catch (WebApiException ex)
            {

                HandleWebApiException(ex);
                return View();
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateShirt(Shirt shirt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await apiExecuter.InvokePut($"Shirts/{shirt.ShirtId}", shirt);
                    return RedirectToAction(nameof(Index));
                }
                catch (WebApiException ex)
                {

                    HandleWebApiException(ex);
                }

            }


            return View(shirt);
        }
        public async Task<IActionResult> DeleteShirt(int shirtId)
        {
            try
            {
                await apiExecuter.InvokeDelete($"Shirts/{shirtId}");
                return RedirectToAction(nameof(Index));
            }
            catch (WebApiException ex)
            {
                HandleWebApiException(ex);

                return View(nameof(Index), await apiExecuter.InvokeGet<List<Shirt>>("Shirts"));
            }

        }
        private void HandleWebApiException(WebApiException ex)
        {
            if (ex.Response != null &&
                          ex.Response.Errors != null &&
                          ex.Response.Errors.Count > 0)
            {
                foreach (var error in ex.Response.Errors)
                {
                    ModelState.AddModelError(error.Key, string.Join("; ", error.Value));
                }

            }
            else if (ex.Response != null)
            {
                ModelState.AddModelError("Error", ex.Response.Title);
            }
            else
            {
                ModelState.AddModelError("Error", ex.Message);
            }
        }
    }
}
