using FluentValidation;
using ITESRC_LIBROSAPI.Models.DTOs;

namespace ITESRC_LIBROSAPI.Models.Validators
{
    public class LibroValidator:AbstractValidator<LibroDto>
    {
        public LibroValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo no debe estar vacio.");
            RuleFor(x => x.Autor).NotEmpty().WithMessage("El autor no debe estar vacio.");
            RuleFor(x => x.Portada).NotEmpty().WithMessage("La imagen de portada no debe estar vacia.");
            RuleFor(x => x.Portada).Must(ValidarURl).WithMessage("Escriba una direccion url de una imagen JPEG");
        }

        private bool ValidarURl(string url)
        {
            return url.StartsWith("https://") && url.EndsWith(".jpg");
        }
    }
}
