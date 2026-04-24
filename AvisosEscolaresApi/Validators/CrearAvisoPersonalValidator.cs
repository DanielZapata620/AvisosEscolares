using AvisosEscolaresApi.Models.DTOs;
using FluentValidation;

namespace AvisosEscolaresApi.Validators
{
    public class CrearAvisoPersonalValidator: AbstractValidator<CrearAvisoPersonalDto>
    {
        public CrearAvisoPersonalValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("El titulo es obligatorio.");
            RuleFor(x => x.Mensaje).NotEmpty().WithMessage("El mensaje es obligatorio.");
            RuleFor(x => x.MaestroId).GreaterThan(0).WithMessage("El MaestroId debe ser mayor que 0.");
            RuleFor(x => x.AlumnoId).GreaterThan(0).WithMessage("El AlumnoId debe ser mayor que 0." );
        }
    }
}
