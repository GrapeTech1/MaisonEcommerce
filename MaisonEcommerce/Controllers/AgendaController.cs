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

            return View();
        }

        [HttpPost]
        public IActionResult CadastrarAgen(Agendamento agendamento)
        {
            int linhasAfetadas = _agendamentoRepositorio.Cadastrar(agendamento);
            ModelState.Remove("NomeCliente");

            if (linhasAfetadas > 0)
            {
                ViewData["Mensagem"] = "Serviço agendado com sucesso!";
                ViewData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                ViewData["Mensagem"] = "O serviço já está com esse horário indisponível, por favor agende outro horário.";
                ViewData["Classe"] = "alert alert-danger";

                ViewBag.clientes = _clienteRepositorio.TodosClientes();
                ViewBag.servicos = _servicoRepositorio.TodosServicos();

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
        public IActionResult EditarAgenda(int id, [Bind("IdAgendamento, IdCliente_Agen, IdServico_Agen, NomeServico, DataHora")] Agendamento agendamento)
        {
            if (id != agendamento.IdAgendamento)
            {
                return BadRequest();
            }

            ModelState.Remove("NomeCliente");
            ModelState.Remove("CPF");

            if (ModelState.IsValid)
            {
                try
                {
                    if (_agendamentoRepositorio.Atualizar(agendamento))
                    {
                        ViewData["Mensagem"] = "Agendamento remarcado com sucesso!";
                        ViewData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        ViewData["Mensagem"] = "Horário indisponível.";
                        ViewData["Classe"] = "alert alert-danger";

                        ViewBag.clientes = _clienteRepositorio.TodosClientes();
                        ViewBag.servicos = _servicoRepositorio.TodosServicos();

                        return View();
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

            ViewData["Mensagem"] = $"Agendamento cancelado com sucesso!  -  {DateTime.Now}";
            ViewData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
