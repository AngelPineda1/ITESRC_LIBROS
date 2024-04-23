using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace ItesrcLibrosMAUI.Models.Entities
{
    [Table("Libros")]
    public class Libro
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Titulo { get; set; }
        [NotNull]
        public string Autor { get; set; }
        [NotNull]
        public string Portada {  get; set; }
        public bool Eliminado { get; set; }
    }
}
