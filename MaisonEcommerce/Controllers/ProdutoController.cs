using MaisonEcommerce.Models;
using MaisonEcommerce.Repositorio;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace MaisonEcommerce.Controllers
{
    public class ProdutoController : Controller
    {
        public readonly ProdutoRepositorio _produtoRepositorio;

        public ProdutoController(ProdutoRepositorio produtoRepositorio)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Produto> produtos = new List<Produto>();

            try
            {
                produtos = await _produtoRepositorio.TodosProdutos();
            }

            catch (Exception ex)
            {
                TempData["Erro"] = $"Erro ao carregar todos os produtos: {ex.Message}";
                Console.WriteLine($"Erro ao carregar todos os produtos: {ex.Message}");
            }

            if (TempData["Cadastro"] != null)
            {
                ViewBag.cadastro = TempData["Cadastro"];
            }

            if (TempData["Erro"] != null)
            {
                ViewBag.erro = TempData["Erro"];
            }
            return View(produtos);
        }

        [HttpGet]
        public IActionResult CadastrarProduto()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CadastrarProduto(Produto produto, IFormFile fotoProduto)
        {
            if (ModelState.IsValid)
            {
                if (fotoProduto != null && fotoProduto.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await fotoProduto.CopyToAsync(ms);
                        produto.Foto = ms.ToArray();
                    }
                    produto.TipoFoto = fotoProduto.ContentType;
                }

                else
                {
                    produto.Foto = null;
                    produto.TipoFoto = null;
                }

                try
                {
                    string resultado = await _produtoRepositorio.Cadastrar(produto);
                    TempData["Cadastro"] = resultado;
                    return RedirectToAction(nameof(Index));
                }

                catch (MySqlException ex)
                {
                    TempData["Erro"] = $"Erro ao cadastrar o produto: {ex.Message}";
                    ModelState.AddModelError("", "Erro no banco de dados ao cadastrar o produto. Por favor, tente novamente mais tarde.");
                }
            }
            return View(produto);
        }

        public async Task<IActionResult> ObterFotoProduto(int id)
        {
            var produto = await _produtoRepositorio.ObterProduto(id);

            if (produto == null || produto.Foto == null || string.IsNullOrEmpty(produto.TipoFoto))
            {
                return NotFound();
            }

            return File(produto.Foto, produto.TipoFoto);
        }

        [HttpGet]
        public async Task<IActionResult> EditarProduto(int id)
        {
            var produto = await _produtoRepositorio.ObterProduto(id);

            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProduto(int id, IFormFile fotoProduto, [Bind("IdProduto, Nome, Descricao, Quantidade, Preco")] Produto produto)
        {
           if (id != produto.IdProduto)
           {
               return BadRequest();
           }

           if (ModelState.IsValid)
           {
                if (fotoProduto != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await fotoProduto.CopyToAsync(ms);
                        produto.Foto = ms.ToArray();
                    }
                    produto.TipoFoto = fotoProduto.ContentType;
                }

                try
                {
                    if (await _produtoRepositorio.Atualizar(produto))
                    {
                        TempData["Mensagem"] = "Produto modificado com sucesso!";
                        TempData["Classe"] = "alert alert-success";
                        return RedirectToAction("Index");
                    }
                }

                catch (Exception)
                {
                    ModelState.AddModelError("", "Erro ao editar o produto.");
                    return View(produto);
                }
            }

            return View(produto);
        }

        public IActionResult ExcluirProduto(int id)
        {
            _produtoRepositorio.Excluir(id);

            TempData["Mensagem"] = $"Produto deletado com sucesso!  -  {DateTime.Now}";
            TempData["Classe"] = "alert alert-success";

            return RedirectToAction(nameof(Index));
        }
    }
}
