using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MundoMarino_Cliente.Controllers
{
    public class UsuarioController : Controller
    {
        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/UsuarioApi";

        

        public ActionResult Create()
        {
            if(HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                if(HttpContext.Session.GetString("LogueadoEsAdmin") == "true")
                {
                    return View();
                }
                else
                {
                    TempData["msgHome"] = "Funcionalidad restringida solo a administradores.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["msgLogin"] = "Debe loguearse.";
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpPost]
        public ActionResult Create(UsuarioModel us)
        {
            try
            {
                UsuarioAliasModel user = new UsuarioAliasModel();
                user.alias = HttpContext.Session.GetString("LogueadoAlias");
                user.obj = us;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                string json = JsonConvert.SerializeObject(user);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;

                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();


                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    UsuarioModel ecosistema = JsonConvert.DeserializeObject<UsuarioModel>(response.Result);

                    TempData["msgHome"] = "Usuario creado correctamente.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msgError"] = "No se pudo crear el usuario.";
                    return RedirectToAction("Error", "Home");

                }
            }
            catch (Exception ex)
            {
                TempData["msgError"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


    }
}
