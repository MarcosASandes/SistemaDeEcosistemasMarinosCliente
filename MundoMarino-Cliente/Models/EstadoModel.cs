using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class EstadoModel
    {
        [Display(Name = "Id")]
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public NombreEstadoEnum nombre { get; set; }
    }
}
