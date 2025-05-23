using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
