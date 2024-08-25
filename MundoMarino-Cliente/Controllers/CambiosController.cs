using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;

namespace MundoMarino_Cliente.Controllers
{
    public class CambiosController : Controller
    {

        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/CambiosApi";

        public IActionResult Index()
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
                    IEnumerable<CambiosModel> cambios = JsonConvert.DeserializeObject<IEnumerable<CambiosModel>>(response.Result);
                    return View(cambios);
                }
                else
                {
                    TempData["msgError"] = "No se pudieron obtener los cambios.";
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
