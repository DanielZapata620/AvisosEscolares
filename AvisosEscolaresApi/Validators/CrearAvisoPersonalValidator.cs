using AvisosEscolaresApi.Models.DTOs;
using FluentValidation;

namespace AvisosEscolaresApi.Validators
{
    public class CrearAvisoPersonalValidator: AbstractValidator<CrearAvisoPersonalDto>
    {
        public CrearAvisoPersonalValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty();
            RuleFor(x => x.Mensaje).NotEmpty();
            RuleFor(x => x.MaestroId).GreaterThan(0);
            RuleFor(x => x.AlumnoId).GreaterThan(0);
        }
    }
}
