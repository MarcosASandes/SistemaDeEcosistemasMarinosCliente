namespace MundoMarino_Cliente.Models
{
    public class UsuarioModel
    {
        public string? token { get; set; }
        public int id { get; set; }
        public string alias { get; set; }
        public string passNormal { get; set; }
        public bool esAdmin { get; set; }
    }
}
