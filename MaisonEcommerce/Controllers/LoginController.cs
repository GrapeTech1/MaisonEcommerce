using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
