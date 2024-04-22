using ITESRC_LIBROSAPI.Models.DTOs;
using ITESRC_LIBROSAPI.Models.Entities;
using ITESRC_LIBROSAPI.Models.Validators;
using ITESRC_LIBROSAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITESRC_LIBROSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        public LibrosController(LibrosRepository repository)
        {
            Repository = repository;
        }

        LibroValidator validator = new();
        public LibrosRepository Repository { get; }
        [HttpPost]
        public IActionResult Post(LibroDto libro)
        {
            //validacion manual
            //if(string.IsNullOrWhiteSpace(libro.Titulo))
            //{
            //    return BadRequest("El titulo esta vacio");
            //}
            var resultados = validator.Validate(libro);
            if (resultados.IsValid)
            {
                Libros libros = new Libros()
                {
                    Id = 0,
                    Eliminado=false,
                    FechaActualizacion=DateTime.Now,
                    Autor=libro.Autor,
                    Titulo=libro.Titulo,
                    Portada=libro.Portada,
                };
                Repository.Insert(libros);
                return Ok(libros);
            }
           
            return BadRequest(resultados.Errors.Select(x=>x.ErrorMessage));
            

        }
    }
}
