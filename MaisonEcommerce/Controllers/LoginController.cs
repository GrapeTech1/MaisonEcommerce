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
        public IActionResult CadastrarUsuario(Usuario usuario)
        {
            if (usuario.Senha != usuario.ConfirmarSenha)
            {
                TempData["Mensagem"] = "Senha inválida, por favor digite a mesma senha nos dois campos.";
                TempData["Classe"] = "alert alert-danger";
                return View(usuario);
            }

            else
            {
                int linhasAfetadas = _loginRepositorio.Cadastrar(usuario);

                if (linhasAfetadas > 0)
                {
                    TempData["Mensagem"] = "Conta cadastrada com sucesso.";
                    TempData["Classe"] = "alert alert-success";
                    return RedirectToAction(nameof(Login));
                }

                else
                {
                    TempData["Mensagem"] = "Email inválido, já tem usuário cadastrado com esse e-mail.";
                    TempData["Classe"] = "alert alert-danger";
                    return View(usuario);
                }
            }

        }

        public IActionResult EditarSenha()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarSenha(string email, [Bind("Nome, Email, Senha, ConfirmarSenha")] Usuario usuario)
        {
            if (email != usuario.Email)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (usuario.Senha != usuario.ConfirmarSenha)
                    {
                        TempData["Mensagem"] = "Senha inválida, por favor digite a nova senha corretamente na confirmação";
                        TempData["Classe"] = "alert alert-danger";
                        return View();
                    }

                    if (_loginRepositorio.Alterar(usuario))
                    {
                        
                        TempData["Mensagem"] = "Senha alterada com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Login));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao atualizar senha.");
                    return View(usuario);
                }
                
            }

            return View(usuario);

        }
    }
}
