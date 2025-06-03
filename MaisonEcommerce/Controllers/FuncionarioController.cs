using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace MaisonEcommerce.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly FuncionarioRepositorio _funcionarioRepositorio;

        public FuncionarioController(FuncionarioRepositorio funcionarioRepositorio)
        {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        public IActionResult Index()
        {
            return View(_funcionarioRepositorio.TodosFuncionarios());
        }

        public IActionResult CadastrarFuncionario()
        {
            return View();
        }

        public IActionResult CadastrarFuncionario(Funcionario funcionario)
        {
            int linhasAfetadas = _funcionarioRepositorio.Cadastrar(funcionario);

            if (linhasAfetadas > 0)
            {
                TempData["Mensagem"] = "Funcionário cadastrado com sucesso!";
                TempData["Classe"] = "alert alert-success";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["Mensagem"] = "O funcionário já existe no sistema.";
                TempData["Classe"] = "alert alert-danger";
                return View();
            }
        }

        public IActionResult EditarFuncionario(int id)
        {
            var funcionario = _funcionarioRepositorio.ObterFuncionario(id);

            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarFuncionario(int id, [Bind("IdFuncionario, CPF, Nome, Cargo, Sexo")] Funcionario funcionario)
        {
            if (id != funcionario.IdFuncionario)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (_funcionarioRepositorio.Atualizar(funcionario))
                    {
                        TempData["Mensagem"] = "Funcionário modificado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction(nameof(Index));
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o funcionário.");
                    return View(funcionario);
                }
            }

            return View(funcionario);
        }

        public IActionResult ExcluirFuncionario(int id)
        {
            _funcionarioRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Funcionário deletado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
