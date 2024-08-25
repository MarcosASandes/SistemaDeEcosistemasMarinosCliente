using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class EcosistemaModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Latitud")]
        public double latitud { get; set; }
        [Display(Name = "Longitud")]
        public double longitud { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Nivel")]
        public int nivelConservacion { get; set; }
        [Display(Name = "Estado")]
        public EstadoModel? estado { get; set; }
        public int idEstado { get; set; }
        [Display(Name = "País")]
        public PaisModel? paisResponsable { get; set; }
        [Display(Name = "Cod. Alpha")]
        public string? codigoAlpha { get; set; }
        public List<EcosistemaAmenazaModel>? _amenazas { get; set; }
        public List<EcosistemaEspecieModel>? _especies { get; set; }
        public List<EcosistemaImagenModel>? _imagenes { get; set; }

        public EcosistemaModel()
        {
            _amenazas = new List<EcosistemaAmenazaModel>();
            _imagenes = new List<EcosistemaImagenModel>();
            _especies = new List<EcosistemaEspecieModel>();
        }


       
        public void AgregarEspecieHabita(EspecieModel e)
        {
            try
            {
                EcosistemaEspecieModel ecoDto = new EcosistemaEspecieModel();
                ecoDto.idEcosistema = this.id;
                ecoDto.idEspecie = e.id;
                ecoDto.loHabita = true;
                this._especies.Add(ecoDto);
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
                EcosistemaAmenazaModel espAm = new EcosistemaAmenazaModel();
                espAm.amenazaId = am.id;
                espAm.ecosistemaId = this.id;

                this._amenazas.Add(espAm);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AgregarImagen(EcosistemaImagenModel img)
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
