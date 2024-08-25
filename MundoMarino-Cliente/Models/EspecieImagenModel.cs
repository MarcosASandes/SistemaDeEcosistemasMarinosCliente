namespace MundoMarino_Cliente.Models
{
    public class EspecieImagenModel
    {
        public int id { get; set; }
        public int idEspecie { get; set; }
        public EspecieModel? esp { get; set; }
        public string ruta { get; set; }

        public EspecieImagenModel()
        {

        }
        
    }
}
