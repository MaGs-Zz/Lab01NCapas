using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using ProxyServer;

namespace WebApplicationOrders.Controllers
{
    public class CustomersController : Controller
    {

        private readonly CustomerProxy _proxy;

        public CustomersController()
        {
            this._proxy = new CustomerProxy();
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _proxy.GetAllAsync();
            return View(customers);
        }

        public IActionResult Create()

        {
            return View();        
        }

        // POST: Customer/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        // O referencias
        public async Task<IActionResult> Create([Bind("Id, FirstName, LastName, City, Country, Phone")] Customer customer)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.CreateAsync(customer);
                    if (result == null)
                    {
                        return RedirectToAction("Error", new { message = "El cliente con el mismo nombre y apellido ya existe." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction($"Error", new { messaje = ex.Message});
                }
            }
            return View(customer);

        }


        //Edit
        //GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _proxy.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }


        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FirstName, LastName, City, Country, Phone")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _proxy.UpdateAsync(id, customer);
                    if (!result)
                    {
                        return RedirectToAction("Error", new { message = "No se puede realizar la edición porque hay duplicidad de nombre con otro cliente." });
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", new { message = ex.Message });
                }
            }
            return View(customer);
        }


        //Get: /Customer/Details/5

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _proxy.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }



        //Delete

        //Get: /Customer/Delete/5

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _proxy.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);

        }

        //POST:  Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _proxy.DeleteAsync(id);
                if (!result)
                {
                    return RedirectToAction("Error", new { message = "No se puede eliminar el cliente por que tiene facturas asociadas." });
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", new { message = ex.Message });
            }
        }
   

        //Error
        public IActionResult Error(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

    }
}