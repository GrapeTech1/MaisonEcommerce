using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class AgendaController : Controller
    {
        private readonly AgendamentoRepositorio _agendamentoRepositorio;
        private readonly ClienteRepositorio _clienteRepositorio;
        private readonly ServicoRepositorio _servicoRepositorio;

        public AgendaController(AgendamentoRepositorio agendamentoRepositorio, ClienteRepositorio clienteRepositorio,
            ServicoRepositorio servicoRepositorio)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _servicoRepositorio = servicoRepositorio;
        }

        public IActionResult Index()
        {
            return View(_agendamentoRepositorio.TodosAgendamentos());
        }

        public IActionResult CadastrarAgen()
        {
            ViewBag.clientes = _clienteRepositorio.TodosClientes();
            ViewBag.servicos = _servicoRepositorio.TodosServicos();

            return View(new Agendamento());
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

            ViewBag.clientes = _clienteRepositorio.TodosClientes();
            ViewBag.servicos = _servicoRepositorio.TodosServicos();

            return View(agendamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarAgenda(int id, [Bind("IdAgendamento, IdCliente_Agen, IdServico_Agen, NomeCliente, NomeServico, DataHora")] Agendamento agendamento)
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
