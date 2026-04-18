using AvisosEscolaresApi.Models.DTOs;
using FluentValidation;

namespace AvisosEscolaresApi.Validators
{
    public class CrearAvisoGeneralValidator : AbstractValidator<CrearAvisoGeneralDto>
    {
        public CrearAvisoGeneralValidator()
        {
            RuleFor(x => x.Titulo)
            .NotEmpty().WithMessage("El título es obligatorio")
            .MaximumLength(200);

            RuleFor(x => x.Mensaje)
                .NotEmpty().WithMessage("El mensaje es obligatorio");

            RuleFor(x => x.FechaCaducidad)
                .GreaterThan(DateTime.Now)
                .WithMessage("La fecha de caducidad debe ser futura");
        }
    }
}
