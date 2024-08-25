using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace MundoMarino_Cliente.Controllers
{
    public class HomeController : Controller
    {
        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/Login";

        private IWebHostEnvironment _environment;
        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            //TempData["msgError"];
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UsuarioModel us)
        {

            try
            {
                Uri uri = new Uri(uriText + "/" + "login");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                string json = JsonConvert.SerializeObject(us);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;

                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    UsuarioModel usuarioLog = JsonConvert.DeserializeObject<UsuarioModel>(response.Result);


                    HttpContext.Session.SetInt32("LogueadoId", usuarioLog.id);
                    HttpContext.Session.SetString("LogueadoAlias", usuarioLog.alias);
                    if(usuarioLog.token != null)
                    {
                        HttpContext.Session.SetString("LogueadoToken", usuarioLog.token);
                    }

                    if (usuarioLog.esAdmin)
                    {
                        HttpContext.Session.SetString("LogueadoEsAdmin", "true");
                    }
                    else
                    {
                        HttpContext.Session.SetString("LogueadoEsAdmin", "false");
                    }

                    TempData["msg"] = "Ingreso correcto. Bienvenido " + usuarioLog.alias;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msgError"] = $"Error al loguearse.";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["msgError"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }




        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                HttpContext.Session.Clear();
            }
            return RedirectToAction("Index");
        }

    }
}