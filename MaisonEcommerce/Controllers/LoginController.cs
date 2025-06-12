using MaisonEcommerce.Models;
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

        public IActionResult CadastrarUsuario ()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarUsuario(Usuario usuario, FormCollection senha)
        {
                TempData["Mensagem"] = "Conta cadastrada com sucesso.";
                TempData["Classe"] = "alert alert-success";

            int linhasAfetadas = _loginRepositorio.Cadastrar(usuario);

            return RedirectToAction(nameof(Login));
            
        }

        public IActionResult EditarSenha(string email)
        {
            var usuario = _loginRepositorio.ObterUsuario(email);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarSenha(string email, [Bind("IdUsuario, Nome, Email, Senha")] Usuario usuario)
        {
            if (email != usuario.Email)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_loginRepositorio.Alterar(usuario))
                    {
                        TempData["Mensagem"] = "Senha alterada com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao alterar senha.");
                    return View(usuario);
                }
                
            }

            return View(usuario);

        }
    }
}
