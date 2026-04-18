using AvisosEscolaresApi.Models.DTOs;
using AvisosEscolaresApi.Models.Entities;
using AvisosEscolaresApi.Repositories;
using FluentValidation;
using static AvisosEscolaresApi.Validators.AgregarAlumnoValidator;

namespace AvisosEscolaresApi.Validators
{
    public class AgregarAlumnoValidator : AbstractValidator<AlumnoCreateDTO>
    {
        private readonly Repository<Alumno> repos;

        public AgregarAlumnoValidator(Repository<Alumno> repos)
        {
            RuleFor(x => x.Nombre).NotEmpty().WithMessage("Escribaa el nombre del alumno").MaximumLength(100).WithMessage("Escriba un nombre de máximo 100 caracteres.");
            RuleFor(x => x.GrupoId).NotEmpty().WithMessage("Seleccione un grupo para el alumno.");
            RuleFor(x => x.Usuario).NotEmpty().WithMessage("Escriba el número de control del alumno.").MaximumLength(8).WithMessage("El número de control debe tener máximo 8 caracteres.").Must(UsuarioRepetido).WithMessage("Ya existe un alumno con el mismo número de control");
            RuleFor(x => x.Contrasena).NotEmpty().WithMessage("Escriba una contraseña para el alumno.").MaximumLength(50).WithMessage("La contraseña debe tener máximo 50 caracteres.");
            this.repos = repos;
        }

        private bool UsuarioRepetido(string usuario)
        {
            return !repos.GetAll().Any(x => x.Usuario == usuario);
        }
    }
}
