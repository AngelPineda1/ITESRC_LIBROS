using FluentValidation;
using ItesrcLibrosMAUI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItesrcLibrosMAUI.Models.Validators
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
            if (url != null)
            {
                return url.StartsWith("https://") && url.EndsWith(".jpg");

            }
            return false;
        }
        }
}
