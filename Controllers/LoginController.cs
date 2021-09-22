using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using TesteCitelSoftware.WebApi.Models;

namespace TesteMazzaFC.WebApi.Controllers
{
    public class LoginController : Controller
    {
        const string URL_BASE = "https://localhost:44308/api/v2/";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginUserViewModel login)
        {

            using (var user = new HttpClient())
            {
                user.BaseAddress = new Uri(URL_BASE);

                var result = await user.PostAsJsonAsync("login", login);

                if (result.IsSuccessStatusCode)
                {
                    var dados = await result.Content.ReadAsAsync<LoginResponseViewModel>();
                    Session["token"] = dados.AccessToken;

                    return RedirectToAction("../Home/Index");
                }
                else
                {
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }

            return View();
        }
        [HttpGet]
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(RegisterUserViewModel register)
        {
            if (register == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                using (var regis= new HttpClient())
                {
                    regis.BaseAddress = new Uri(URL_BASE);

                    var result = await regis.PostAsJsonAsync("registrar", register);
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    var error = await result.Content.ReadAsAsync<MenssagensViewModel>();
                    ModelState.AddModelError(string.Empty, error.Errors.FirstOrDefault());
                }
            }
            return View(register);
        }
    }
}