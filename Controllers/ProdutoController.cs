using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TesteCitelSoftware.WebApi.Models;

namespace TesteCitelSoftware.WebApi.Controllers
{
    public class ProdutoController : Controller
    {
        const string URL_BASE = "https://localhost:44308/api/v2/";

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<ProdutoViewModel> produtos = null;

            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri(URL_BASE);
                prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var result = await prod.GetAsync("produtos");

                if (result.IsSuccessStatusCode)
                {
                    produtos = await result.Content.ReadAsAsync<IList<ProdutoViewModel>>();
                    return View(produtos);
                }
                else
                {
                    if (result.ReasonPhrase == "Unauthorized")
                    {
                        return View("Unauthorized");
                    }
                }
            }
            return View(produtos);
        }

        [HttpGet]
        public async Task<ActionResult> Pesquisar(string nome)
        {
            IEnumerable<ProdutoViewModel> produtos = null;
            return View("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Cadastro()
        {
            await CreateCategoriaViewBag();

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cadastro(ProdutoViewModel produto)
        {
            if (produto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                using (var prod = new HttpClient())
                {
                    prod.BaseAddress = new Uri(URL_BASE);
                    prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                    var result = await prod.PostAsJsonAsync("produtos", produto);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            await CreateCategoriaViewBag();
            return View(produto);
        }

        [HttpGet]
        public async Task<ActionResult> Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProdutoViewModel produto = null;

            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri($"{URL_BASE}produtos/{id}");
                prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var responseTask = await prod.GetAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    produto = await responseTask.Content.ReadAsAsync<ProdutoViewModel>();
                    await CreateCategoriaViewBag();
                }
            }
            return View(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Alterar(ProdutoViewModel produto)
        {
            if (produto == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri(URL_BASE);
                prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var result = await prod.PutAsJsonAsync<ProdutoViewModel>($"produtos/{produto.Id}", produto);

                if (result.IsSuccessStatusCode) return RedirectToAction("Index");
                else
                {
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            await CreateCategoriaViewBag();
            return View(produto);
        }

        [HttpGet]
        public async Task<ActionResult> Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProdutoViewModel produto = null;

            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri($"{URL_BASE}produtos/{id}");
                prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var responseTask = await prod.GetAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    produto = await responseTask.Content.ReadAsAsync<ProdutoViewModel>();
                    await CreateCategoriaViewBag();
                }
            }
            return View(produto);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var prod = new HttpClient())
            {
                prod.BaseAddress = new Uri($"{URL_BASE}produtos/{id}");
                prod.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var responseTask = await prod.DeleteAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            throw new HttpException();
        }

        private async Task CreateCategoriaViewBag()
        {
            IEnumerable<CategoriaViewModel> categorias = null;

            using (var catego = new HttpClient())
            {
                catego.BaseAddress = new Uri(URL_BASE);
                catego.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var result = await catego.GetAsync("categorias");

                if (result.IsSuccessStatusCode)
                {
                    categorias = await result.Content.ReadAsAsync<IList<CategoriaViewModel>>();
                    ViewBag.categorias = categorias;
                }
            }
        }
    }
}