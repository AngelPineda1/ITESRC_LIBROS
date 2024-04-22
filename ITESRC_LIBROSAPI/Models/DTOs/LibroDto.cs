namespace ITESRC_LIBROSAPI.Models.DTOs
{
    public class LibroDto
    {
        public int? Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string Portada { get; set; } = null!;
        public bool Eliminado { get; set; }
    }
}
