using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class CambiosModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Responsable")]
        public string nombreResponsable { get; set; }
        [Display(Name = "Fecha y hora")]
        public DateTime fechaHora { get; set; }
        [Display(Name = "Id entidad")]
        public int idEntidad { get; set; }
        [Display(Name = "Tipo entidad")]
        public string tipoEntidad { get; set; }

    }
}
