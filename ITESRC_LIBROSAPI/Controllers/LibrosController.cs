using FluentValidation;
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
                    FechaActualizacion=DateTime.UtcNow,
                    Autor=libro.Autor,
                    Titulo=libro.Titulo,
                    Portada=libro.Portada,
                };
                Repository.Insert(libros);
                return Ok(libros);
            }
           
            return BadRequest(resultados.Errors.Select(x=>x.ErrorMessage));
            

        }
        [HttpGet("{fecha?}/{hora?}/{minutos?}")]
        public IActionResult Get(DateTime? fecha,int hora=0,int minutos=0)
        {
            if (fecha != null)
            {

                fecha = new DateTime(fecha.Value.Year, fecha.Value.Month, fecha.Value.Day, hora, minutos, 0);
            }

            var libros = Repository.GetAll()
                .Where(x=>fecha==null || x.FechaActualizacion >fecha)
                .OrderBy(x=>x.Titulo)
                .Select(x=>new LibroDto
                {
                    Id=x.Id,
                    Titulo=x.Titulo,
                    Eliminado=x.Eliminado,
                    Portada=x.Portada,
                    Autor=x.Autor,
                   Fecha=x.FechaActualizacion
                });

            return Ok(libros);
        }

        [HttpPut("{id}")]
        public IActionResult Put(LibroDto dto)
        {
            var resultados=validator.Validate(dto);
            if (resultados.IsValid)
            {
                var entidad=Repository.Get(dto.Id??0);
                if (entidad == null ||entidad.Eliminado==true)
                {
                    return NotFound();
                }
                else {
                    entidad.Titulo=dto.Titulo;
                    entidad.Autor=dto.Autor;
                    entidad.Portada=dto.Portada;
                    entidad.FechaActualizacion=DateTime.UtcNow;
                    Repository.Update(entidad);
                    return Ok();
                }

                
            }
            return BadRequest(resultados.Errors.Select(x => x.ErrorMessage));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entidad=Repository.Get(id);
            if (entidad == null ||entidad.Eliminado == true)
            {
                return NotFound();
            }
            entidad.Eliminado=true; 
            entidad.FechaActualizacion = DateTime.UtcNow;
            Repository.Update(entidad);

            return Ok();
        }
    }
}
