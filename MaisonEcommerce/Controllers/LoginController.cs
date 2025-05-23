using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginRepositorio _loginRepositorio;

        public LoginController(LoginRepositorio loginRepositorio)
        {
            _loginRepositorio = loginRepositorio;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string senha)
        {
            var usuario = _loginRepositorio.ObterUsuario(email);

            if (usuario != null && usuario.Senha == senha)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email ou senha incorretos.");
            return View();
        }


    }
}
