using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class PacoteController : Controller
    {
        private readonly PacoteRepositorio _pacoteRepositorio;

        public PacoteController(PacoteRepositorio pacoteRepositorio)
        {
            _pacoteRepositorio = pacoteRepositorio;
        }

        public IActionResult Index()
        {
            return View(_pacoteRepositorio.TodosPacotes());
        }

        public IActionResult CadastrarPacote()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarPacote(Pacote pacote)
        {
            int linhasAfetadas = _pacoteRepositorio.Cadastrar(pacote);

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Pacote cadastrado com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["Mensagem"] = "O pacote já existe no sistema.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarPacote(int id)
        {
            var pacote = _pacoteRepositorio.ObterPacote(id);

            if (pacote == null)
            {
                return NotFound();
            }

            return View(pacote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarPacote(int id, [Bind("IdPacote, Nome, Descricao, Preco")] Pacote pacote)
        {
            if (id != pacote.IdPacote)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_pacoteRepositorio.Atualizar(pacote))
                    {
                        TempData["Mensagem"] = "Pacote modificado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o pacote.");
                    return View(pacote);
                }
            }

            return View(pacote);
        }

        public IActionResult ExcluirPacote(int id)
        {
            _pacoteRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Pacote deletado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
