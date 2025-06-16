using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class PlanoController : Controller
    {
        private readonly PlanoRepositorio _planoRepositorio;

        public PlanoController(PlanoRepositorio planoRepositorio)
        {
            _planoRepositorio = planoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_planoRepositorio.TodosPlanos());
        }

        public IActionResult CadastrarPlano()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarPlano(Plano plano)
        {
            int linhasAfetadas = _planoRepositorio.Cadastrar(plano);

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Plano cadastrado com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["Mensagem"] = "O plano já existe no sistema.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarPlano(int id)
        {
            var plano = _planoRepositorio.ObterPlano(id);

            if (plano == null)
            {
                return NotFound();
            }

            return View(plano);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarPlano(int id, [Bind("IdPlano, Nome, Descricao, Duracao, Preco")] Plano plano)
        {
            if (id != plano.IdPlano)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_planoRepositorio.Atualizar(plano))
                    {
                        TempData["Mensagem"] = "Plano alterado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o plano.");
                    return View(plano);
                }
            }

            return View(plano);
        }

        public IActionResult ExcluirPlano(int id)
        {
            _planoRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Plano deletado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
