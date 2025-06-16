using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class ServicoPlanoController : Controller
    {
        private readonly ServicoPlanoRepositorio _servicoplanoRepositorio;
        private readonly ServicoRepositorio _servicoRepositorio;
        private readonly PlanoRepositorio _planoRepositorio;

        public ServicoPlanoController(ServicoPlanoRepositorio servicoplanoRepositorio, 
            ServicoRepositorio servicoRepositorio, PlanoRepositorio planoRepositorio)
        {
            _servicoplanoRepositorio = servicoplanoRepositorio;
            _servicoRepositorio = servicoRepositorio;
            _planoRepositorio = planoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_servicoplanoRepositorio.TodosServicoPlano());
        }

        public IActionResult CadastrarServicoPlano()
        {
            ViewBag.servicos = _servicoRepositorio.TodosServicos();
            ViewBag.planos = _planoRepositorio.TodosPlanos();

            return View(new Servico_Plano());
        }

        [HttpPost]
        public IActionResult CadastrarServicoPlano(Servico_Plano servicoPlano)
        {
            int linhasAfetadas = _servicoplanoRepositorio.Cadastrar(servicoPlano);

            if(linhasAfetadas > 0)
            {
                ViewData["Mensagen"] = "Serviço inserido em plano com sucesso!";
                ViewData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                ViewData["Mensagem"] = "O serviço já foi adicionado anteriormente nesse plano!";
                ViewData["Classe"] = "alert alert-danger";
                return View(); 
            }
        }

        public IActionResult EditarServicoPlano(int id)
        {
            var servicoPlano = _servicoplanoRepositorio.ObterServicoPlano(id);

            if (servicoPlano == null)
            {
                return NotFound();
            }

            ViewBag.servicos = _servicoRepositorio.TodosServicos();
            ViewBag.planos = _planoRepositorio.TodosPlanos();

            return View(servicoPlano);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult EditarServicoPlano(int id, [Bind("IdServicoPlano, IdServico, IdPlano, NomeServico, NomePlano")] Servico_Plano servicoPlano)
        {
            if (id != servicoPlano.IdServicoPlano)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (_servicoplanoRepositorio.Atualizar(servicoPlano))
                    {
                        TempData["Mensagem"] = "Servico inserido em pacote com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar pacote");
                    return View(servicoPlano);
                }
            }
            return View(servicoPlano);
        }
        public IActionResult ExcluirServicoPlano(int id)
        {
            _servicoplanoRepositorio.Excluir(id);


            TempData["Mensagem"] = $"Serviço retirado do plano com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
