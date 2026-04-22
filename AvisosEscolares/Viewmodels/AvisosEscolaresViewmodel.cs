using AvisosEscolares.Models.DTOs;
using AvisosEscolares.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvisosEscolares.Viewmodels
{
    public class AvisosEscolaresViewmodel: INotifyPropertyChanged
    {

        public LoginDTO LoginDTO { get; set; } = new LoginDTO();

        public ObservableCollection<AvisoGeneralDetallesMaestroDTO> AvisosGenerales { get; set; } = new();

        public ObservableCollection<AvisoPersonalDetallesMaestroDTO> AvisosPersonales { get; set; } = new();
        public ObservableCollection<AlumnoDetallesListaDTO> ListaAlumnos { get; set; } = new();
        public ObservableCollection<AvisoPersonalListaAlumnoDTO> ListaAvisosPersonales { get; set; } = new();
        public int CantAvisosPersonalesNuevos { get; set; }

        public MaestroDTO maestro { get; set; }

        //Hacer un mapper para poner un aviso y que de ahi seaque el crear y ek editar aviso general
        public CrearAvisoGeneralDto NuevoAvisoGeneral { get; set; } = new CrearAvisoGeneralDto();
        public CrearAvisoPersonalDto NuevoAvisoPersonal { get; set; } =new CrearAvisoPersonalDto();

        public int AlumnoSeleccionado { get; set; } = new();

        public AlumnoCreateDTO NuevoAlumno { get; set; } = new AlumnoCreateDTO();

        public AlumnoDTO alumno { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand CambiarVistaCommand { get; set; }

        public ICommand AgregarAlumnoCommand{ get; set; }
        public ICommand VerListaAlumnosCommand { get; set; }

        public ICommand CrearAvisoGeneralCommand { get; set; }
        public ICommand CrearAvisoPersonalCommand { get; set; }

        
        public ICommand MostrarAvisosGeneralesMaestroCommand { get; set; }

        public ICommand MostrarCrearAvisoPersonalCommand { get; set; }
        public ICommand MostrarAvisosPersonalesMaestroCommand { get; set; }
        public ICommand MostrarAvisosPersonalesAlumnoCommand { get; set; }


        AvisosEscolaresServices service = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public AvisosEscolaresViewmodel()
        {
            LoginCommand = new Command(Login);
            MostrarAvisosGeneralesMaestroCommand = new Command(MostrarAvisosGeneralesMaestro);
            MostrarAvisosPersonalesMaestroCommand = new Command<int>(MostrarAvisosPersonalesMaestro);
            MostrarCrearAvisoPersonalCommand=new Command<int>(MostrarCrearAvisoPersonal);
            CambiarVistaCommand = new Command<string>(CambiarVista);
            CrearAvisoGeneralCommand = new Command(CrearAvisoGeneral);
            CrearAvisoPersonalCommand = new Command(CrearAvisoPersonal);
            VerListaAlumnosCommand = new Command(VerListaAlumnos);
            AgregarAlumnoCommand = new Command(AgregarAlumno);
            MostrarAvisosPersonalesAlumnoCommand = new Command(MostrarAvisosPersonalesAlumno);
        }

        private async void MostrarAvisosPersonalesAlumno()
        {
            ListaAvisosPersonales.Clear();
            var avisos = await service.ObtenerAvisosPersonalesAlumno(alumno.Id);
         
            foreach (var aviso in avisos)
            {
                ListaAvisosPersonales.Add(aviso);
            }
            var avisosNuevos = avisos.Where(a => a.Estado == "Nuevo").Select(a => a.AvisoId).ToList();
            CantAvisosPersonalesNuevos = avisosNuevos.Count;
            PropertyChanged?.Invoke(this,new(nameof(CantAvisosPersonalesNuevos)));
            await service.MarcarAvisosPersonalesLeidos(avisosNuevos, alumno.Id);

        }

       
        
            
        
        

        private async void MostrarCrearAvisoPersonal(int idAlumno)
        {
            AlumnoSeleccionado = idAlumno;
            NuevoAvisoPersonal.AlumnoId = AlumnoSeleccionado;
            await Shell.Current.GoToAsync("crearAvisoPersonal");
        }

        private async void MostrarAvisosPersonalesMaestro(int idAlumno)
        {
           AvisosPersonales.Clear();
            var avisos = await service.ObtenerAvisosPersonalesPorAlumno(idAlumno);
            foreach (var aviso in avisos)
            {
                AvisosPersonales.Add(aviso);
            }
             await Shell.Current.GoToAsync("avisosPersonalesMaestro");
        }

        private async void CrearAvisoPersonal()
        {
            NuevoAvisoPersonal.MaestroId = (int)maestro.Id;
            NuevoAvisoPersonal.AlumnoId = AlumnoSeleccionado;
            await service.CrearAvisoPersonal(NuevoAvisoPersonal);
            VerListaAlumnos();
        }
       

        private async void AgregarAlumno()
        {
            NuevoAlumno.GrupoId = (int)maestro.GrupoId;
            NuevoAlumno.Contrasena = "ESCOLARES";
            await service.CrearAlumno(NuevoAlumno);
            VerListaAlumnos();
        }

        private async void VerListaAlumnos()
        {
            ListaAlumnos.Clear();
            var alumnos = await service.GetAlumnosByGrupo((int)maestro.GrupoId);
            foreach(var a in alumnos)
            {
              ListaAlumnos.Add(a);
            }
            await Shell.Current.GoToAsync("listaAlumnos");
        }

        private async void CrearAvisoGeneral()
        {
            await service.CrearAvisoGeneral(NuevoAvisoGeneral);
            MostrarAvisosGeneralesMaestro();

        }

        private async void CambiarVista(string obj)
        {
            await Shell.Current.GoToAsync(obj);
        }

        private async void MostrarAvisosGeneralesMaestro()
        {
            AvisosGenerales.Clear();
            var avisos = await service.ObtenerAvisosGeneralesVigentes();
            foreach (var aviso in avisos)
            {
                AvisosGenerales.Add(aviso);
            }
            await Shell.Current.GoToAsync("avisosGeneralesMaestro");

        }

        public async void Login()
        {
            var result = await service.Login(LoginDTO);
            if(result.Rol== "Alumno")
            {
                
               alumno = result.Alumno;
                await Shell.Current.GoToAsync("dashboardAlumno");
            }
            else if(result.Rol == "Maestro")
            {
                // Navegar a la vista del maestro
               maestro = result.Maestro;
               await Shell.Current.GoToAsync("dashboardMaestro");
            }

        }



        


    }
}
