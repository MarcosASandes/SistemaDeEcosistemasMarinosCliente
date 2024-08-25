using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class EspecieModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Nombre científico")]
        public string nombreCientifico { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Peso")]
        public double peso { get; set; }
        [Display(Name = "Longitud")]
        public double longitud { get; set; }
        [Display(Name = "Estado")]
        public EstadoModel? estado { get; set; }
        public int idEstado { get; set; }
        [Display(Name = "Nivel")]
        public int nivelConservacion { get; set; }
        public List<EspecieAmenazaModel>? _amenazas { get; set; }
        public List<EspecieImagenModel>? _imagenes { get; set; }
        public List<EcosistemaEspecieModel>? _ecosistemas { get; set; }

        public EspecieModel()
        {
            _amenazas = new List<EspecieAmenazaModel>();
            _imagenes = new List<EspecieImagenModel>();
            _ecosistemas = new List<EcosistemaEspecieModel>();
        }


        public void AgregarEcosistemaNoHabita(EcosistemaModel e)
        {
            try
            {
                EcosistemaEspecieModel ecoDto = new EcosistemaEspecieModel();
                ecoDto.idEcosistema = e.id;
                ecoDto.idEspecie = this.id;
                ecoDto.loHabita = false;
                this._ecosistemas.Add(ecoDto);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AgregarEcosistemaHabita(EcosistemaModel e)
        {
            try
            {
                EcosistemaEspecieModel ecoDto = new EcosistemaEspecieModel();
                ecoDto.idEcosistema = e.id;
                ecoDto.idEspecie = this.id;
                ecoDto.loHabita = true;
                this._ecosistemas.Add(ecoDto);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void AgregarAmenaza(AmenazaModel am)
        {
            try
            {
                EspecieAmenazaModel espAm = new EspecieAmenazaModel();
                espAm.idAmenaza = am.id;
                espAm.idEspecie = this.id;

                this._amenazas.Add(espAm);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AgregarImagen(EspecieImagenModel img)
        {
            try
            {
                this._imagenes.Add(img);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
