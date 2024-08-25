namespace MundoMarino_Cliente.Models
{
    public class EcosistemaImagenModel
    {
        public int id { get; set; }
        public int idEcosistema { get; set; }
        public EcosistemaModel? eco { get; set; }
        public string ruta { get; set; }
       
    }
}
