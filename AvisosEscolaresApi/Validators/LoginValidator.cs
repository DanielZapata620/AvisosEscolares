using AvisosEscolaresApi.Models.DTOs;
using FluentValidation;
using static AvisosEscolaresApi.Models.DTOs.LoginDTO;

namespace AvisosEscolaresApi.Validators
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Usuario)
                .NotEmpty().WithMessage("El Usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El Usuario no puede exceder los 50 caracteres.");
            RuleFor(x => x.Contrasena)
                .NotEmpty().WithMessage("La Contraseña es obligatoria.")
                .MaximumLength(50).WithMessage("La Contraseña no puede exceder los 50 caracteres.");
        }
    }
}
