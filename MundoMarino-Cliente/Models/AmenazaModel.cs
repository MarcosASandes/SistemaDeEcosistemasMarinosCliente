using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class AmenazaModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Descripción")]
        public string descripcion { get; set; }
        [Display(Name = "Grado de peligro")]
        public int gradoPeligro { get; set; }
    }
}
