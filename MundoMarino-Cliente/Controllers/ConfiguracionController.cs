using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MundoMarino_Cliente.Controllers
{
    public class ConfiguracionController : Controller
    {
        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/ConfiguracionApi";

        // GET: EcosistemaController
        public ActionResult Index()
        {
            try
            {
                Uri uri = new Uri(uriText);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    IEnumerable<ConfiguracionModel> topes = JsonConvert.DeserializeObject<IEnumerable<ConfiguracionModel>>(response.Result);
                    return View(topes);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["msgError"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        private ConfiguracionModel ObtenerConfigPorNombre(string nombre)
        {
            Uri uri = new Uri(uriText + "/" + nombre);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                ConfiguracionModel topes = JsonConvert.DeserializeObject<ConfiguracionModel>(response.Result);
                return topes;
            }
            else
            {
                return null;
            }
        }


        public ActionResult Edit(string nombreAtributo)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.atributoName = nombreAtributo;
                return View();
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpPost]
        public ActionResult Edit(string nombre, int topeInferior, int topeSuperior)
        {
            try
            {
                ConfiguracionModel config = ObtenerConfigPorNombre(nombre);

                if (topeInferior > topeSuperior)
                {
                    int aux = topeInferior;
                    topeInferior = topeSuperior;
                    topeSuperior = aux;
                }

                config.topeSuperior = topeSuperior;
                config.topeInferior = topeInferior;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
                string json = JsonConvert.SerializeObject(config);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    ConfiguracionModel equipo = JsonConvert.DeserializeObject<ConfiguracionModel>(response.Result);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msgError"] = "No se pudo editar las configuración";
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
