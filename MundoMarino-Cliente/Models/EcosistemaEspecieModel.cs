namespace MundoMarino_Cliente.Models
{
    public class EcosistemaEspecieModel
    {

        public int idEcosistema { get; set; }
        public EcosistemaModel? ecosistema { get; set; }
        public int idEspecie { get; set; }
        public EspecieModel? especie { get; set; }
        public bool loHabita { get; set; }
        
    }
}
