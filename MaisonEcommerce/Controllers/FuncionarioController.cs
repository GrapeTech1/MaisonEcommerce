using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
