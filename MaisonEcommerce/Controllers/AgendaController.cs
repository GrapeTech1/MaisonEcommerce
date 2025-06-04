using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;

        public AgendaController(AgendamentoRepositorio agendamentoRepositorio)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_agendamentoRepositorio.TodosAgendamentos());
        }

        public IActionResult CadastrarAgen()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarAgen(Agendamento agendamento)
        {
            int linhasAfetadas = _agendamentoRepositorio.Cadastrar(agendamento);

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Agendamento cadastrado com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["Mensagem"] = "O horário está indisponível, por favor agende outro horário.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarAgenda(int id)
        {
            var agendamento = _agendamentoRepositorio.ObterAgendamento(id);

            if (agendamento == null)
            {
                return NotFound();
            }

            return View(agendamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCliente(int id, [Bind("IdAgendamento, IdCliente_Agen, IdServico_Agen")] Agendamento agendamento)
        {
            if (id != agendamento.IdAgendamento)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_agendamentoRepositorio.Atualizar(agendamento))
                    {
                        TempData["Mensagem"] = "Agendamento remarcado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao remarcar o agendamento.");
                    return View(agendamento);
                }
            }

            return View(agendamento);
        }

        public IActionResult ExcluirAgenda(int id)
        {
            _agendamentoRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Agendamento apagado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
