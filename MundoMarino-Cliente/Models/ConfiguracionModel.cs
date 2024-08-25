using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MundoMarino_Cliente.Models
{
    public class ConfiguracionModel
    {
        [Display(Name = "Atributo")]
        public string nombreAtributo { get; set; }
        [Display(Name = "Mínimo")]
        public int topeInferior { get; set; }
        [Display(Name = "Máximo")]
        public int topeSuperior { get; set; }

    }
}
