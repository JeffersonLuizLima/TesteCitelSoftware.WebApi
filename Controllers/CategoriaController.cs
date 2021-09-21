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
    
    public class CategoriaController : Controller
    {
        const string URL_BASE = "https://localhost:44308/api/v2/";

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            IEnumerable<CategoriaViewModel> categorias = null;

            using (var cat = new HttpClient())
            {
                cat.BaseAddress = new Uri(URL_BASE);
                cat.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var result = await cat.GetAsync("categorias");

                if (result.IsSuccessStatusCode)
                {
                    categorias = await result.Content.ReadAsAsync<IList<CategoriaViewModel>>();
                    return View(categorias);
                }
                else
                {
                    if (result.ReasonPhrase == "Unauthorized")
                    {
                        return View("Error");
                    }
                }

            }
                return View(categorias);
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Cadastro(CategoriaViewModel categoria)
        {
            if (categoria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                using (var cat = new HttpClient())
                {
                    cat.BaseAddress = new Uri(URL_BASE);
                    cat.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                    var result = await cat.PostAsJsonAsync("categorias", categoria);

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<ActionResult> Alterar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoriaViewModel categoria = null;

            using (var cat = new HttpClient())
            {
                cat.BaseAddress = new Uri($"{URL_BASE}categorias/{id}");
                cat.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var responseTask = await cat.GetAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    categoria = await responseTask.Content.ReadAsAsync<CategoriaViewModel>();
                }
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<ActionResult> Alterar(CategoriaViewModel categoria)
        {
            if (categoria == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var cat = new HttpClient())
            {
                cat.BaseAddress = new Uri(URL_BASE);
                cat.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var result = await cat.PutAsJsonAsync<CategoriaViewModel>($"categorias/{categoria.Id}", categoria);

                if (result.IsSuccessStatusCode) return RedirectToAction("Index");
                else
                {
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (var cat = new HttpClient())
            {
                cat.BaseAddress = new Uri($"{URL_BASE}categorias/{id}");
                cat.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)(Session["token"] ?? string.Empty));

                var responseTask = await cat.DeleteAsync("");

                if (responseTask.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            throw new HttpException();
        }
    }
}