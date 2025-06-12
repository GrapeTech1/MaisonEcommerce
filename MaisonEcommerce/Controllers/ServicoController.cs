using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace MaisonEcommerce.Controllers
{
    public class ServicoController : Controller
    {
        private readonly ServicoRepositorio _servicoRepositorio;

        public ServicoController(ServicoRepositorio servicoRepositorio)
        {
            _servicoRepositorio = servicoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_servicoRepositorio.TodosServicos());
        }

        public IActionResult CadastrarServico()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarServico(Servico servico)
        {
            int linhasAfetadas = _servicoRepositorio.Cadastrar(servico);

            if (linhasAfetadas > 0)
            {
                try
                {
                    string texto = Convert.ToString(servico.Preco);
                    texto = texto.Replace(",", ".");
                    
                    decimal teste = Convert.ToDecimal(texto);

                    TempData["Mensagem"] = "O serviço foi cadastrado com sucesso!";
                    TempData["Classe"] = "alert alert-success";
                    return RedirectToAction(nameof(Index));
                }

                catch
                {
                    TempData["Mensagem"] = "Preço inválido.";
                    TempData["Classe"] = "alert alert-danger";
                    return View();
                }
                
            }

            else
            {
                TempData["Mensagem"] = "O serviço já existe.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarServico(int id)
        {
            var servico = _servicoRepositorio.ObterServico(id);

            if (servico == null)
            {
                return NotFound();
            }
            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarServico(int id, [Bind("IdServico, Nome, Descricao, Preco")] Servico servico)
        {
            if (id != servico.IdServico)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_servicoRepositorio.Atualizar(servico))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o serviço.");
                    return View(servico);
                }
            }
            return View(servico);
        }

        public IActionResult Excluir(int id)
        {
            _servicoRepositorio.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
