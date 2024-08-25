using Humanizer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MundoMarino_Cliente.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MundoMarino_Cliente.Controllers
{
    public class EspecieController : Controller
    {
        private HttpClient cliente = new HttpClient();
        private string uriText = "http://localhost:5287/api/EspecieApi";
        private string uriAmenazas = "http://localhost:5287/api/AmenazaApi";
        private string uriEcosistemas = "http://localhost:5287/api/EcosistemaApi";

        private IWebHostEnvironment _environment;
        public EspecieController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // GET: EspecieController
        public ActionResult Index()
        {
            Uri uri = new Uri(uriText);
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
                TempData["msgError"] = "Ocurrió un problema al cargar las especies (Index-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        //// GET: EspecieController/Details/5
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
                    EspecieModel especies = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                    string rutaDeserializada = GetPrimerImagenEsp(id);
                    ViewBag.primerImagen = rutaDeserializada;
                    return View(especies);
                }
                else
                {
                    TempData["msgError"] = $"Ocurrió un problema al cargar la especie con id {id} (Details-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message + " (Details-Especie)";
                return RedirectToAction("Error", "Home");
            }

        }

        //GET: Obtener primer imagen para details.
        private string GetPrimerImagenEsp(int id)
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
                    EspecieImagenModel primerImagenEco = JsonConvert.DeserializeObject<EspecieImagenModel>(response.Result);
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

        public ActionResult MostrarEspeciesPorNombre()
        {
            Uri uri = new Uri(uriText);
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
                TempData["msgError"] = "Error al cargar las especies. (FiltroNombreGet-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult MostrarEspeciesPorNombre(string nombreBuscado)
        {
            if (nombreBuscado != null)
            {
                Uri uri = new Uri(uriText + "/" + "GetByName" + "/" + nombreBuscado);
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
                    TempData["msgError"] = "Error al cargar las especies. (FiltroNombrePost-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                ViewBag.msgNull = "No se ha dado un filtro correcto.";
                return View();
            }
        }

        public ActionResult MostrarEspeciesPorPeso()
        {
            Uri uri = new Uri(uriText);
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
                TempData["msgError"] = "Error al cargar las especies. (FiltroPesoGet-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult MostrarEspeciesPorPeso(int minimo, int maximo)
        {
            if (minimo != null && maximo != null)
            {
                if (minimo > maximo)
                {
                    int aux = minimo;
                    minimo = maximo;
                    maximo = aux;
                }

                Uri uri = new Uri(uriText + "/" + "minimo=" + minimo + "/" + "maximo=" + maximo);
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
                    TempData["msgError"] = "Error al cargar las especies. (FiltroPesoPost-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                ViewBag.msgNull = "No se ha dado un filtro correcto.";
                return View();
            }
        }


        public ActionResult MostrarEspeciesPorPesoYNombre()
        {
            Uri uri = new Uri(uriText);
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
                TempData["msgError"] = "Error al cargar las especies. (FiltroPesoYNombreGet-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public ActionResult MostrarEspeciesPorPesoYNombre(int minimo, int maximo, string nombre)
        {
            if (minimo != null && maximo != null && nombre != null)
            {
                if (minimo > maximo)
                {
                    int aux = minimo;
                    minimo = maximo;
                    maximo = aux;
                }

                Uri uri = new Uri(uriText + "/" + "nombre=" + nombre + "/" + "minimo=" + minimo + "/" + "maximo=" + maximo);
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
                    TempData["msgError"] = "Error al cargar las especies. (FiltroPesoYNombrePost-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                ViewBag.msgNull = "No se ha dado un filtro correcto.";
                return View();
            }
        }


        public ActionResult MostrarEcosistemasInadecuados(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetEcosistemasInadecuados" + "/" + id);
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
                TempData["msgError"] = "Error al cargar los ecosistemas inadecuados. (MostrarEcosistemasInadecuados-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult MostrarEcosistemasHabitables(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetEcosistemasHabitables" + "/" + id);
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
                TempData["msgError"] = "Error al cargar los ecosistemas habitables. (MostrarEcosistemasHabitables-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult MostrarEspeciesEnPeligro()
        {

            //Consumo de API con HttpClient (C#).
            //Uri uri = new Uri(uriText + "/" + "GetEspeciesEnPeligro");
            //HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            //Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            //respuesta.Wait();

            //if (respuesta.Result.IsSuccessStatusCode)
            //{
            //    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
            //    response.Wait();
            //    IEnumerable<EspecieModel> ecosistemas = JsonConvert.DeserializeObject<IEnumerable<EspecieModel>>(response.Result);
            //    return View(ecosistemas);
            //}
            //else
            //{
            //    TempData["msgError"] = "Error al cargar las especies en peligro. (MostrarEspeciesEnPeligro-Especie)";
            //    return RedirectToAction("Error", "Home");
            //}


            //Utilizando Fetch en JS.
            return View();
        }


        public ActionResult MostrarEcosistemasDeEspecie(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetEcosistemasDeEspecie" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<EcosistemaModel> especies = JsonConvert.DeserializeObject<IEnumerable<EcosistemaModel>>(response.Result);
                return View(especies);
            }
            else
            {
                TempData["msgError"] = $"Error al cargar los ecosistemas de la especie con id {id}. (MostrarEcosistemas-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }


        public ActionResult MostrarAmenazas(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetAmenazasDeEspecie" + "/" + id);
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
                TempData["msgError"] = $"Error al cargar las amenazas de la especie con id {id}. (MostrarAmenazas-Especie)";
                return RedirectToAction("Error", "Home");
            }
        }




        public ActionResult Create()
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
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
        public ActionResult Create(IFormFile imagen, EspecieModel esp)
        {
            try
            {
                if (esp == null || imagen == null)
                {
                    return View();
                }

                AsignarIdEstado(esp);

                EspecieAliasModel especie = new EspecieAliasModel();
                especie.alias = HttpContext.Session.GetString("LogueadoAlias");
                especie.obj = esp;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Post, uri);

                string json = JsonConvert.SerializeObject(especie);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;

                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();


                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                    GuardarLaImagen(imagen, equipo);
                    return RedirectToAction("AgregarAmenazaEspecie", "Especie", new { id = equipo.id });
                }
                else
                {
                    TempData["msgError"] = $"Error al crear la especie. (Create-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.msg = e.Message;
                return View();
            }
        }



        public ActionResult AgregarAmenazaEspecie(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.idEspecie = id;
                return View(ObtenerTodasLasAmenazas());
            }
            else
            {
                TempData["msgError"] = "Funcionalidad restringida para logueados.";
                return RedirectToAction("Error", "Home");
            }
        }



        [HttpPost]
        public ActionResult AgregarAmenazaEspecie(int idEspecie, List<int> amenazasElegidas)
        {
            try
            {
                EspecieModel esp = ObtenerEspecie(idEspecie);
                List<EspecieAmenazaModel> amenazasAgregadas = new List<EspecieAmenazaModel>();
                foreach (int idAm in amenazasElegidas)
                {
                    EspecieAmenazaModel espAmenazaModel = new EspecieAmenazaModel();
                    espAmenazaModel.idEspecie = esp.id;
                    espAmenazaModel.idAmenaza = idAm;
                    amenazasAgregadas.Add(espAmenazaModel);
                }

                esp._amenazas.AddRange(amenazasAgregadas);

                EspecieAliasModel espec = new EspecieAliasModel();
                espec.alias = HttpContext.Session.GetString("LogueadoAlias");
                espec.obj = esp;

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                Uri uri = new Uri(uriText + "/" + "AgregarAmenazaEspecie");
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
                string json = JsonConvert.SerializeObject(espec);
                HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                solicitud.Content = contenido;
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                    return RedirectToAction(nameof(Details), new { id = espec.obj.id });
                }
                else
                {
                    TempData["msgError"] = "Error al intentar agregar la amenaza. (AgregarAmenazaEspecie-Ecosistema)";
                    return RedirectToAction("Error", "Home");
                }

            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }

        private List<EspecieModel> ObtenerEspecies()
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
                    List<EspecieModel> especie = JsonConvert.DeserializeObject<List<EspecieModel>>(response.Result);
                    return especie;
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


        private List<EspecieModel> ObtenerEspeciesPorEcosistema(int id)
        {
            Uri uri = new Uri(uriEcosistemas + "/" + "GetEspDeEco" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                List<EspecieModel> especies = JsonConvert.DeserializeObject<List<EspecieModel>>(response.Result);
                return especies;
            }
            else
            {
                return null;
            }
        }

        public ActionResult ListarEspeciesDeUnEco()
        {
            ViewBag.ecosistemas = ObtenerTodosLosEcosistemas();
            return View(ObtenerEspecies());
        }

        [HttpPost]
        public ActionResult ListarEspeciesDeUnEco(int ecosistemaId)
        {
            ViewBag.ecosistemas = ObtenerTodosLosEcosistemas();
            return View(ObtenerEspeciesPorEcosistema(ecosistemaId));
        }



        private void GuardarLaImagen(IFormFile imagen, EspecieModel esp)
        {
            try
            {
                if (esp != null)
                {
                    if (GuardarImagen(imagen, esp))
                    {

                        EspecieAliasModel objAlias = new EspecieAliasModel();
                        objAlias.alias = HttpContext.Session.GetString("LogueadoAlias");
                        objAlias.obj = esp;

                        Uri uri = new Uri(uriText);
                        HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);

                        string json = JsonConvert.SerializeObject(objAlias);
                        HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                        solicitud.Content = contenido;

                        Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                        respuesta.Wait();


                        if (respuesta.Result.IsSuccessStatusCode)
                        {
                            Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                            response.Wait();
                            EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        private bool GuardarImagen(IFormFile imagen, EspecieModel e)
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
                (rutaFisicaWwwRoot, "img", "especies", nombreImagen);
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

                    EspecieImagenModel ecoImg = new EspecieImagenModel();
                    ecoImg.ruta = nombreImagen;
                    ecoImg.idEspecie = e.id;
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


        private string SetNombreDeImagen(int idEsp)
        {
            List<string> rutasEsp = GetImagenesEspecie(idEsp);

            int siguienteNumero = 1;
            if (rutasEsp.Count > 0)
            {
                int numeroMasAlto = rutasEsp.Select(ruta => int.Parse(ruta.Substring(ruta.IndexOf('_') + 1))).Max();
                siguienteNumero = numeroMasAlto + 1;
            }

            string nuevoNombre = $"{idEsp}_{siguienteNumero:D3}";

            return nuevoNombre;
        }


        private List<string> GetImagenesEspecie(int id)
        {
            try
            {
                Uri uri = new Uri(uriText + "/" + "GetImagenesEspecie" + "/" + id);
                HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
                Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                    response.Wait();
                    List<EspecieImagenModel> imagenes = JsonConvert.DeserializeObject<List<EspecieImagenModel>>(response.Result);
                    List<string> retorno = new List<string>();
                    foreach (EspecieImagenModel e in imagenes)
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


        private void AsignarIdEstado(EspecieModel e)
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



        public ActionResult Edit(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.IdEsp = id;
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
        public ActionResult Edit(int idEspecie, string? nameCientifico, string? nameCasual, string? nameDesc)
        {
            try
            {
                EspecieModel espEncontrada = ObtenerEspecie(idEspecie);
                if (espEncontrada != null)
                {
                    if (!string.IsNullOrEmpty(nameCientifico))
                    {
                        espEncontrada.nombreCientifico = nameCientifico;
                    }

                    if (!string.IsNullOrEmpty(nameCasual))
                    {
                        espEncontrada.nombre = nameCasual;
                    }

                    if (!string.IsNullOrEmpty(nameDesc))
                    {
                        espEncontrada.descripcion = nameDesc;
                    }

                    EspecieAliasModel objAlias = new EspecieAliasModel();
                    objAlias.alias = HttpContext.Session.GetString("LogueadoAlias");
                    objAlias.obj = espEncontrada;

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                    Uri uri = new Uri(uriText + "/" + "EditarEspecie");
                    HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);

                    string json = JsonConvert.SerializeObject(objAlias);
                    HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    solicitud.Content = contenido;

                    Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                    respuesta.Wait();


                    if (respuesta.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                        response.Wait();
                        EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                        return RedirectToAction(nameof(Details), new { id = objAlias.obj.id });
                    }
                    else
                    {
                        return RedirectToAction("Error", "Home");
                    }

                }
                else
                {
                    TempData["msgError"] = $"Error al editar la especie. (Edit-Especie)";
                    return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception e)
            {
                TempData["msgError"] = e.Message;
                return RedirectToAction("Error", "Home");
            }
        }


        private EspecieModel ObtenerEspecie(int id)
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
                    EspecieModel especie = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                    return especie;
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
        //        ViewBag.especieId = id;
        //        return View(ObtenerTodasLasAmenazas());
        //    }
        //    else
        //    {
        //        TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
        //        return RedirectToAction("Login", "Home");
        //    }
        //}

        //[HttpPost]
        //public ActionResult AsociarAmenazas(int idEspecie, int idAmenaza)
        //{
        //    try
        //    {
        //        EspecieModel espEncontrada = ObtenerEspecie(idEspecie);
        //        AmenazaModel amEncontrada = ObtenerAmenazaPorId(idAmenaza);


        //        if (!ContieneLaAmenaza(idEspecie, idAmenaza))
        //        {
        //            espEncontrada.AgregarAmenaza(amEncontrada);

        //            EspecieAliasModel objAlias = new EspecieAliasModel();
        //            objAlias.alias = HttpContext.Session.GetString("LogueadoAlias");
        //            objAlias.obj = espEncontrada;

        //            cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

        //            Uri uri = new Uri(uriText + "/" + "AgregarAmenazaEspecie");
        //            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
        //            string json = JsonConvert.SerializeObject(objAlias);
        //            HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
        //            solicitud.Content = contenido;
        //            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
        //            respuesta.Wait();

        //            if (respuesta.Result.IsSuccessStatusCode)
        //            {
        //                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
        //                response.Wait();
        //                EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
        //                return RedirectToAction(nameof(Details), new { id = objAlias.obj.id });
        //            }
        //            else
        //            {
        //                return RedirectToAction("Error", "Home");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.msg = $"El ecosistema {espEncontrada.nombre} ya contiene esa amenaza.";
        //            return View();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        #endregion

        private bool ContieneLaAmenaza(int idEsp, int idAmenaza)
        {
            bool ret = false;
            IEnumerable<AmenazaModel> amenazasDeEsp = ObtenerAmenazasDeEsp(idEsp);
            if (amenazasDeEsp != null)
            {
                foreach (AmenazaModel a in amenazasDeEsp)
                {
                    if (a.id == idAmenaza)
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        private IEnumerable<AmenazaModel> ObtenerAmenazasDeEsp(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetAmenazasDeEspecie" + "/" + id);
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





        private EcosistemaModel ObtenerEcosistemaPorId(int id)
        {
            Uri uri = new Uri(uriEcosistemas + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                EcosistemaModel eco = JsonConvert.DeserializeObject<EcosistemaModel>(response.Result);
                return eco;
            }
            else
            {
                return null;
            }
        }


        private IEnumerable<EcosistemaModel> ObtenerEcosistemasHabitables(int id)
        {
            Uri uri = new Uri(uriText + "/" + "GetEcosistemasHabitables" + "/" + id);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<EcosistemaModel> ecosistemas = JsonConvert.DeserializeObject<IEnumerable<EcosistemaModel>>(response.Result);
                return ecosistemas;
            }
            else
            {
                return null;
            }
        }

        private bool ContieneEcoHabitable(int idEco, int idEsp)
        {
            try
            {
                bool ret = false;
                IEnumerable<EcosistemaModel> ecosistemasHabitables = ObtenerEcosistemasHabitables(idEsp);
                foreach (EcosistemaModel e in ecosistemasHabitables)
                {
                    if (e.id == idEco)
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

        private IEnumerable<EcosistemaModel> ObtenerTodosLosEcosistemas()
        {
            Uri uri = new Uri(uriEcosistemas);
            HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Get, uri);
            Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
            respuesta.Wait();

            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                response.Wait();
                IEnumerable<EcosistemaModel> especies = JsonConvert.DeserializeObject<IEnumerable<EcosistemaModel>>(response.Result);
                return especies;
            }
            else
            {
                return null;
            }
        }


        public ActionResult AgregarEcosistemaHabitable(int id)
        {
            if (HttpContext.Session.GetInt32("LogueadoId") != null)
            {
                ViewBag.idEspecie = id;
                return View(ObtenerTodosLosEcosistemas());
            }
            else
            {
                TempData["msgLogueado"] = "Debe loguearse para acceder a esa funcionalidad.";
                return RedirectToAction("Login", "Home");
            }
        }


        [HttpPost]
        public ActionResult AgregarEcosistemaHabitable(int idEspecie, int idEcosistema)
        {

            try
            {
                if (!ContieneEcoHabitable(idEcosistema, idEspecie))
                {
                    EspecieModel espEncontrada = ObtenerEspecie(idEspecie);
                    EcosistemaModel ecoEncontrado = ObtenerEcosistemaPorId(idEcosistema);

                    espEncontrada.AgregarEcosistemaNoHabita(ecoEncontrado);

                    EspecieAliasModel objAlias = new EspecieAliasModel();
                    objAlias.alias = HttpContext.Session.GetString("LogueadoAlias");
                    objAlias.obj = espEncontrada;

                    cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("LogueadoToken"));

                    Uri uri = new Uri(uriText + "/" + "AgregarHabitableEspecie");
                    HttpRequestMessage solicitud = new HttpRequestMessage(HttpMethod.Put, uri);
                    string json = JsonConvert.SerializeObject(objAlias);
                    HttpContent contenido = new StringContent(json, Encoding.UTF8, "application/json");
                    solicitud.Content = contenido;
                    Task<HttpResponseMessage> respuesta = cliente.SendAsync(solicitud);
                    respuesta.Wait();

                    if (respuesta.Result.IsSuccessStatusCode)
                    {
                        Task<string> response = respuesta.Result.Content.ReadAsStringAsync();
                        response.Wait();
                        EspecieModel equipo = JsonConvert.DeserializeObject<EspecieModel>(response.Result);
                        return RedirectToAction(nameof(Details), new { id = objAlias.obj.id });
                    }
                    else
                    {
                        TempData["msgError"] = "Error al intentar agregar un ecosistema habitable.";
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    ViewBag.Msg = "El ecosistema ya esta agregado.";
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
