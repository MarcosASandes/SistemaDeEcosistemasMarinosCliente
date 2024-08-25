namespace MundoMarino_Cliente.Models
{
    public class EspecieAmenazaModel
    {
        public int idEspecie { get; set; }
        public EspecieModel? especie { get; set; }
        public int idAmenaza { get; set; }
        public AmenazaModel? amenaza { get; set; }
       
    }
}
