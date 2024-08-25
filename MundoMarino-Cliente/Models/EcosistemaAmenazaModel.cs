namespace MundoMarino_Cliente.Models
{
    public class EcosistemaAmenazaModel
    {
        public int ecosistemaId { get; set; }
        public EcosistemaModel? ecosistema { get; set; }
        public int amenazaId { get; set; }
        public AmenazaModel? amenaza { get; set; }

        public EcosistemaAmenazaModel()
        {
        }

    }
}
