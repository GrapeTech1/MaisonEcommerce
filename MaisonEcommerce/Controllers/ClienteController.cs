using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteRepositorio _clienteRepositorio;

        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index()
        {
            return View(_clienteRepositorio.TodosClientes());
        }
        
        public IActionResult CadastrarCliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastrarCliente(Cliente cliente)
        {
            int linhasAfetadas = _clienteRepositorio.Cadastrar(cliente);

            if (cliente.Idade < 18)
            {
                TempData["Mensagem"] = "O cliente deve ter pelo menos 18 anos para ser cadastrado no sistema.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Cliente cadastrado com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["Mensagem"] = "O cliente já existe no sistema.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarCliente(int id)
        {
            var cliente = _clienteRepositorio.ObterCliente(id);

           

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarCliente(int id, [Bind("IdCliente, Nome, Telefone, Idade, Sexo")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return BadRequest();
            }

            ModelState.Remove("CPF");

            if (ModelState.IsValid)
            {
                try
                {
                    if (cliente.Idade < 18)
                    {
                        TempData["Mensagem"] = "Idade inválida, o cliente tem que ser maior de idade!";
                        TempData["Classe"] = "alert alert-danger";
                        return View();
                    }

                    if (_clienteRepositorio.Atualizar(cliente))
                    {
                        TempData["Mensagem"] = "Cliente modificado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }

                    else
                    {
                        TempData["Mensagem"] = "CPF inválido, esse CPF já foi cadastrado no sistema anteriormente.";
                        TempData["Classe"] = "alert alert-danger";
                        return View();
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o cliente.");
                    return View(cliente);
                }
            }

            return View(cliente);
        }

        public IActionResult ExcluirCliente(int id)
        {
            _clienteRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Cliente deletado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
