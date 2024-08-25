using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace IUWeb.Controllers
{
    public class EcosistemaController : Controller
    {
        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/EcosistemaApi";
        private string uriPais = "http://localhost:5287/api/PaisApi";
        private string uriAmenazas = "http://localhost:5287/api/AmenazaApi";
        private string uriEspecies = "http://localhost:5287/api/EspecieApi";

        private IWebHostEnvironment _environment;
        public EcosistemaController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

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
                    IEnumerable<EcosistemaModel> ecosistemas = JsonConvert.DeserializeObject<IEnumerable<EcosistemaModel>>(response.Result);
                    return View(ecosistemas);
                }
                else
                {
                    TempData["msgError"] = "Error al mostrar los ecosistemas. (Index-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["msgError"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }


        }


        // GET: EcosistemaController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel ecosistemas = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                    string rutaDeserializada = GetPrimerImagenEco(id);
                    ViewBag.primerImagen = rutaDeserializada;
                    return View(ecosistemas);
                }
                else
                {
                    TempData["msgError"] = "Error al mostrar el detalle. (Details-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }

        }

        //GET: Obtener primer imagen para details.
        private string GetPrimerImagenEco(int id)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + "GetPrimerImagen" + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaImagenModel primerImagenEco = JsonConvert.DeserializeObject<EcosistemaImagenModel>(response.Result);
                    return primerImagenEco.ruta;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult MostrarEspeciesEcosistema(int idEco)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + "GetEspDeEco" + "/" + idEco);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    IEnumerable<EspecieModel> especies = JsonConvert.DeserializeObject<IEnumerable<EspecieModel>>(response.Result);
                    return View(especies);
                }
                else
                {
                    TempData["msgError"] = "Error al mostrar las especies. (MostrarEspeciesEcosistema-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["msgError"] = ex.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult MostrarAmenazas(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetAmenazasDeEcosistema" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<AmenazaModel> amenazas = JsonConvert.DeserializeObject<IEnumerable<AmenazaModel>>(response.Result);
                return View(amenazas);
            }
            else
            {
                TempData["msgError"] = "Error al mostrar las amenazas. (MostrarAmenazas-Ecosistema)";
                return RedirectToAction("Error", "Home");
            }
        }


        private IEnumerable<AmenazaModel> ObtenerAmenazasDeEco(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetAmenazasDeEcosistema" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<AmenazaModel> amenazas = JsonConvert.DeserializeObject<IEnumerable<AmenazaModel>>(response.Result);
                return amenazas;
            }
            else
            {
                return null;
            }
        }


        public ActionResult Create()
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.paises = ObtenerPaises();
                return View();
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: EquipoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormFile imagen, EcosistemaModel eco, string paisSel)
        {
            try
            {
                if (eco == null || imagen == null)
                {
                    return View();
                }

                AsignarIdEstado(eco);
                eco.codigoAlpha = paisSel;

                EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
                ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
                ecosis.obj = eco;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                string json = JsonConvert.SerializeObject(ecosis);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;

                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();


                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel ecosistema = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                    GuardarLaImagen(imagen, ecosistema);
                    return RedirectToAction("AgregarAmenazaEcosistema", "Ecosistema", new { id = ecosistema.id });
                }
                else
                {
                    TempData["msgError"] = $"Error al crear el ecosistema. (Create-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult AgregarAmenazaEcosistema(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.idEcosistema = id;
                return View(ObtenerTodasLasAmenazas());
            }
            else
            {
                TempData["msgError"] = "Funcionalidad restringida para logueados.";
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public ActionResult AgregarAmenazaEcosistema(int idEcosistema, List<int> amenazasElegidas)
        {
            try
            {
                EcosistemaModel eco = ObtenerEcosistema(idEcosistema);
                List<EcosistemaAmenazaModel> amenazasAgregadas = new List<EcosistemaAmenazaModel>();
                foreach (int idAm in amenazasElegidas)
                {
                    EcosistemaAmenazaModel ecosistemaAmenazaModel = new EcosistemaAmenazaModel();
                    ecosistemaAmenazaModel.ecosistemaId = eco.id;
                    ecosistemaAmenazaModel.amenazaId = idAm;
                    amenazasAgregadas.Add(ecosistemaAmenazaModel);
                }

                eco._amenazas.AddRange(amenazasAgregadas);

                EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
                ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
                ecosis.obj = eco;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText + "/" + "AgregarAmenazaEcosistema");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
                string json = JsonConvert.SerializeObject(ecosis);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel equipo = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                    return RedirectToAction(nameof(Details), new { id = ecosis.obj.id });
                }
                else
                {
                    TempData["msgError"] = "Error al intentar agregar la amenaza. (AgregarAmenazaEcosistema-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        private IEnumerable<PaisModel> ObtenerPaises()
        {
            try
            {
                Uri uriNueva = new Uri(uriPais);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uriNueva);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    IEnumerable<PaisModel> paises = JsonConvert.DeserializeObject<IEnumerable<PaisModel>>(response.Result);
                    return paises;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        private void GuardarLaImagen(IFormFile imagen, EcosistemaModel eco)
        {
            try
            {
                if (eco != null)
                {
                    if (GuardarImagen(imagen, eco))
                    {
                        EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
                        ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
                        ecosis.obj = eco;

                        Uri uri = new Uri(uriText);
                        HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);

                        string json = JsonConvert.SerializeObject(ecosis);
                        HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                        solicitud.Content = contenido;

                        Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                        respuesta.Wait();


                        if (respuesta.Result.IsSuccessStatusCode)
                        {
                            Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                            response.Wait();
                            EcosistemaModel equipo = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        private bool GuardarImagen(IFormFile imagen, EcosistemaModel e)
        {
            if (imagen == null || e == null) return false;
            // SUBIR LA IMAGEN
            //ruta física de wwwroot
            string rutaFisicaWwwRoot = _environment.WebRootPath;

            //Asignacion del nombre que corresponde a la imagen
            string extension = imagen.FileName.Split(".").Last();
            if (extension == "png" || extension == "jpg" || extension == "jpge")
            {
                string nombreImagen = SetNombreDeImagen(e.id) + "." + extension;
                //ruta donde se guardan las fotos de las personas
                string rutaFisicaFoto = Path.Combine
                (rutaFisicaWwwRoot, "img", "ecosistemas", nombreImagen);
                //FileStream permite manejar archivos
                try
                {
                    //el método using libera los recursos del objeto FileStream al finalizar
                    using (FileStream f = new FileStream(rutaFisicaFoto, FileMode.Create))
                    {
                        //Para archivos grandes o varios archivos usar la versión
                        //asincrónica de CopyTo. Sería: await imagen.CopyToAsync (f);
                        imagen.CopyTo(f);
                    }
                    //GUARDAR EL NOMBRE DE LA IMAGEN SUBIDA EN EL OBJETO
                    //tengo que crear el VO de la imagen debería validar todo esto RECORDAR MAS TARDE

                    EcosistemaImagenModel ecoImg = new EcosistemaImagenModel();
                    ecoImg.ruta = nombreImagen;
                    ecoImg.idEcosistema = e.id;
                    e.AgregarImagen(ecoImg);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        private string SetNombreDeImagen(int idEco)
        {
            List<string> rutasEco = GetImagenesEcosistema(idEco);
            int siguienteNumero = 1;
            if (rutasEco.Count > 0)
            {
                int numeroMasAlto = rutasEco.Select(ruta => int.Parse(ruta.Substring(ruta.IndexOf('_') + 1))).Max();
                siguienteNumero = numeroMasAlto + 1;
            }
            string nuevoNombre = $"{idEco}_{siguienteNumero:D3}";

            return nuevoNombre;
        }


        private List<string> GetImagenesEcosistema(int id)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + "GetImagenesEcosistema" + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    List<EcosistemaImagenModel> imagenes = JsonConvert.DeserializeObject<List<EcosistemaImagenModel>>(response.Result);
                    List<string> retorno = new List<string>();
                    foreach (EcosistemaImagenModel e in imagenes)
                    {
                        retorno.Add(e.ruta);
                    }
                    return retorno;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void AsignarIdEstado(EcosistemaModel e)
        {
            Uri uri = new Uri(uriText + "/" + "GetEstadoPorNivel" + "/" + e.nivelConservacion);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                int idEstado = JsonConvert.DeserializeObject<int>(response.Result);
                e.idEstado = idEstado;
            }
        }


        public ActionResult Delete(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.IdEco = id;
                Uri uri = new Uri(uriText + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel eco = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);

                    return View(eco);
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }


        //// POST: EcosistemaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, bool confirmacion)
        {
            try
            {
                if (confirmacion)
                {
                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));
                    Uri uri = new Uri(uriText + "/" + id + "/" + HttpContext.Session.GetString("LogueadoAlias"));
                    HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Delete, uri);
                    Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                    respuesta.Wait();

                    if (respuesta.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                        response.Wait();
                        return RedirectToAction("Index", "Ecosistema");
                    }
                    else
                    {
                        TempData["msgError"] = "No se ha podido borrar el ecosistema.";
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    ViewBag.errorMsg = "Debe confirmar la eliminación.";
                    ViewBag.IdEco = id;
                    return View();
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.IdEco = id;
                return View();
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }

        // POST: EspecieController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string nameEco, string descEco, int IdEco)
        {
            try
            {
                EcosistemaModel ecoBuscado = ObtenerEcosistema(IdEco);
                if (nameEco != null)
                {
                    ecoBuscado.nombre = nameEco;
                }
                if (descEco != null)
                {
                    ecoBuscado.descripcion = descEco;
                }

                EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
                ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
                ecosis.obj = ecoBuscado;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText + "/" + "EditarEcosistema");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);

                string json = JsonConvert.SerializeObject(ecosis);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;

                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel equipo = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                    return RedirectToAction(nameof(Details), new { id = ecosis.obj.id });
                }
                else
                {
                    TempData["msgError"] = "Error al intentar editar el ecosistema. (Edit-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        private EcosistemaModel ObtenerEcosistema(int id)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EcosistemaModel ecosistema = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                    return ecosistema;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private AmenazaModel ObtenerAmenazaPorId(int id)
        {
            try
            {
                Uri uri = new Uri(uriAmenazas + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    AmenazaModel amenaza = JsonConvert.DeserializeObject<AmenazaModel>(response.Result);
                    return amenaza;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region AsociarAmenaza - No utilizado
        //public ActionResult AsociarAmenazas(int id)
        //{
        //    if (HttpContext.Session.GetInt32("LogueadoId") != null)
        //    {
        //        ViewBag.idEco = id;
        //        return View(ObtenerTodasLasAmenazas());
        //    }
        //    else
        //    {
        //        TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
        //        return RedirectToAction("Login", "Home");
        //    }
        //}

        //[HttpPost]
        //public ActionResult AsociarAmenazas(int idEcosistema, int idAmenaza)
        //{
        //    try
        //    {
        //        EcosistemaModel ecoEncontrado = ObtenerEcosistema(idEcosistema);
        //        AmenazaModel amEncontrada = ObtenerAmenazaPorId(idAmenaza);
        //        if (!ContieneLaAmenaza(idEcosistema, idAmenaza))
        //        {
        //            ecoEncontrado.AgregarAmenaza(amEncontrada);
        //            EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
        //            ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
        //            ecosis.obj = ecoEncontrado;

        //            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

        //            Uri uri = new Uri(uriText + "/" + "AgregarAmenazaEcosistema");
        //            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
        //            string json = JsonConvert.SerializeObject(ecosis);
        //            HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
        //            solicitud.Content = contenido;
        //            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
        //            respuesta.Wait();

        //            if (respuesta.Result.IsSuccessStatusCode)
        //            {
        //                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
        //                response.Wait();
        //                EcosistemaModel equipo = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
        //                return RedirectToAction(nameof(Details), new { id = ecosis.obj.id });
        //            }
        //            else
        //            {
        //                TempData["msgError"] = "Error al intentar agregar la amenaza. (AsociarAmenaza-Ecosistema)";
        //                return RedirectToAction("Error", "Home");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.msg = $"El ecosistema {ecoEncontrado.nombre} ya contiene esa amenaza.";
        //            return View();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        TempData["msgError"] = e.Message;
        //        return RedirectToAction("Error", "Home");
        //    }
        //}
        #endregion


        private bool ContieneLaAmenaza(int idEco, int idAmenaza)
        {
            bool ret = false;
            IEnumerable<AmenazaModel> amenazasDeEco = ObtenerAmenazasDeEco(idEco);
            if (amenazasDeEco != null)
            {
                foreach (AmenazaModel a in amenazasDeEco)
                {
                    if (a.id == idAmenaza)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        private List<AmenazaModel> ObtenerTodasLasAmenazas()
        {
            Uri uri = new Uri(uriAmenazas);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                List<AmenazaModel> amenazas = JsonConvert.DeserializeObject<List<AmenazaModel>>(response.Result);
                return amenazas;
            }
            else
            {
                return null;
            }
        }





        private IEnumerable<EspecieModel> ObtenerTodasLasEspecies()
        {
            Uri uri = new Uri(uriEspecies);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<EspecieModel> especies = JsonConvert.DeserializeObject<IEnumerable<EspecieModel>>(response.Result);
                return especies;
            }
            else
            {
                return null;
            }
        }

        private EspecieModel ObtenerEspeciePorId(int idEspecie)
        {
            Uri uri = new Uri(uriEspecies + "/" + idEspecie);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                EspecieModel especies = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                return especies;
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<EspecieModel> ObtenerEspeciesDeEcosistema(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetEspDeEco" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<EspecieModel> especies = JsonConvert.DeserializeObject<IEnumerable<EspecieModel>>(response.Result);
                return especies;
            }
            else
            {
                return null;
            }
        }

        private bool ContieneEspecie(int idEco, int idEsp)
        {
            try
            {
                bool ret = false;
                IEnumerable<EspecieModel> especiesDelEco = ObtenerEspeciesDeEcosistema(idEco);
                foreach (EspecieModel e in especiesDelEco)
                {
                    if (e.id == idEsp)
                    {
                        ret = true;
                    }
                }
                return ret;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IEnumerable<AmenazaModel> ObtenerAmenazasDeEspecieId(int id)
        {
            Uri uri = new Uri(uriEspecies + "/" + "GetAmenazasDeEspecie" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<AmenazaModel> amenazas = JsonConvert.DeserializeObject<IEnumerable<AmenazaModel>>(response.Result);
                return amenazas;
            }
            else
            {
                return null;
            }
        }

        private bool CompartenAmenazas(int idEco, int idEsp)
        {
            bool ret = false;
            IEnumerable<AmenazaModel> amenazasEco = ObtenerAmenazasDeEco(idEco);
            IEnumerable<AmenazaModel> amenazasEsp = ObtenerAmenazasDeEspecieId(idEsp);
            foreach (AmenazaModel aEco in amenazasEco)
            {
                foreach (AmenazaModel aEsp in amenazasEsp)
                {
                    if (aEco.id == aEsp.id)
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }


        public ActionResult AsociarEspecie(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.idEcosistema = id;
                return View(ObtenerTodasLasEspecies());
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult AsociarEspecie(int idEco, int idEsp)
        {
            try
            {

                if (!ContieneEspecie(idEco, idEsp))
                {
                    EcosistemaModel ecoEncontrado = ObtenerEcosistema(idEco);
                    EspecieModel espEncontrada = ObtenerEspeciePorId(idEsp);

                    if (ecoEncontrado.nivelConservacion < espEncontrada.nivelConservacion)
                    {
                        throw new Exception($"El nivel de conservación de {ecoEncontrado.nombre} es menor que el de {espEncontrada.nombre}");
                    }

                    if (CompartenAmenazas(idEco, idEsp))
                    {
                        throw new Exception($"{ecoEncontrado.nombre} y {espEncontrada.nombre} comparten amenazas.");
                    }

                    ecoEncontrado.AgregarEspecieHabita(espEncontrada);

                    EcosistemaAliasModel ecosis = new EcosistemaAliasModel();
                    ecosis.alias = HttpContext.Session.GetString("LogueadoAlias");
                    ecosis.obj = ecoEncontrado;

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                    Uri uri = new Uri(uriText + "/" + "AgregarEspecieEcosistema");
                    HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
                    string json = JsonConvert.SerializeObject(ecosis);
                    HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    solicitud.Content = contenido;
                    Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                    respuesta.Wait();

                    if (respuesta.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                        response.Wait();
                        EcosistemaModel equipo = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                        return RedirectToAction(nameof(Details), new { id = ecosis.obj.id });
                    }
                    else
                    {
                        TempData["msgError"] = "Error al asociar la especie. (AsociarEspecie-Ecosistema)";
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    ViewBag.msg = "Ya existe esta especie en este ecosistema.";
                    return View();
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

    }
}
