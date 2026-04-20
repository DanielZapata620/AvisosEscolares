using AvisosEscolares.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvisosEscolares
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Routing.RegisterRoute("dashboardMaestro", typeof(MaestroDasboard));
            Routing.RegisterRoute("avisosGeneralesMaestro", typeof(AvisosGeneralesMaestroPage));
            Routing.RegisterRoute("crearAvisoGeneral", typeof(AgregarAvisoGeneralPage));
            Routing.RegisterRoute("listaAlumnos", typeof(ListaAlumnosPage));
            Routing.RegisterRoute("AgregarAlumno", typeof(AgregarAlumno));
            Routing.RegisterRoute("crearAvisoPersonal", typeof(AgregarAvisoPersonalView));
            Routing.RegisterRoute("avisosPersonalesMaestro", typeof(AvisosPersonalesMaestro));


        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}