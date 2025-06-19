using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class ServPacoteController : Controller
    {
        private readonly ServPacoteRepositorio _servPacoteRepositorio;
        private readonly ServicoRepositorio _servicoRepositorio;
        private readonly PacoteRepositorio _pacoteRepositorio;

        public ServPacoteController(ServPacoteRepositorio servPacoteRepositorio, ServicoRepositorio servicoRepositorio,
            PacoteRepositorio pacoteRepositorio)
        {
            _servPacoteRepositorio = servPacoteRepositorio;
            _servicoRepositorio = servicoRepositorio;
            _pacoteRepositorio = pacoteRepositorio;
        }

        public IActionResult Index()
        {
            return View(_servPacoteRepositorio.TodosServPacote());
        }

        public IActionResult CadastrarServPacote()
        {
            ViewBag.servicos = _servicoRepositorio.TodosServicos();
            ViewBag.pacotes = _pacoteRepositorio.TodosPacotes();

            return View();
        }

        [HttpPost]
        public IActionResult CadastrarServPacote(Servico_Pacote servPacote)
        {
            int linhasAfetadas = _servPacoteRepositorio.Cadastrar(servPacote);

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Serviço inserido em pacote com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                ViewData["Mensagem"] = "O serviço já foi adicionado anteriormente nesse pacote.";
                ViewData["Classe"] = "alert alert-danger";

                ViewBag.servicos = _servicoRepositorio.TodosServicos();
                ViewBag.pacotes = _pacoteRepositorio.TodosPacotes();

                return View();
            }
            
        }

        public IActionResult EditarServPacote(int id)
        {
            var servPacote = _servPacoteRepositorio.ObterServPacote(id);

            if (servPacote == null)
            {
                return NotFound();
            }

            ViewBag.servicos = _servicoRepositorio.TodosServicos();
            ViewBag.pacotes = _pacoteRepositorio.TodosPacotes();

            return View(servPacote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarServPacote(int id, [Bind("IdServicoPacote, IdServico, IdPacote, NomeServico, NomePacote")] Servico_Pacote servPacote)
        {
            if (id != servPacote.IdServicoPacote)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_servPacoteRepositorio.Atualizar(servPacote))
                    {
                        TempData["Mensagem"] = "Serviço ou pacote alterado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        ViewData["Mensagem"] = "O servico que você está tentando inserir no pacote já foi inserido anteriormente.";
                        ViewData["Classe"] = "alert alert-danger";
                        return View();
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar pacote");
                    return View(servPacote);
                }
            }

            return View(servPacote);
        }

        public IActionResult ExcluirServPacote(int id)
        {
            _servPacoteRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Serviço retirado do pacote com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}

