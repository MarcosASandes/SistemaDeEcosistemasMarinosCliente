using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class PaisModel
    {
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "CCA3")]
        public string codigoAlpha { get; set; }
    }
}
