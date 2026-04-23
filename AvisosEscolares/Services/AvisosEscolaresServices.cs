using AvisosEscolares.Models.DTOs;

using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;


namespace AvisosEscolares.Services
{
    public class AvisosEscolaresServices
    {
        
        string baseUrl = "https://localhost:7251/";
        HttpClient client;
        

        public AvisosEscolaresServices()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
        }

        public void SetToken(string token)
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<AlumnoDetallesListaDTO>> GetAlumnosByGrupo(int grupoId)
        {
            var response = await client.GetAsync($"api/alumnos/grupo");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDetallesListaDTO>>();
            }
            return new List<AlumnoDetallesListaDTO>();
        }

        public async Task<(LoginResponseDTO? data, string? error)> Login(LoginDTO loginDto)
        {
            var response = await client.PostAsJsonAsync("api/auth/", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
                SetToken(loginResponse.Token);
                return (loginResponse, null);
            }

            
            var error = await response.Content.ReadAsStringAsync();
            return (null, error);
        }


        public async Task<List<AvisoGeneralDetallesMaestroDTO>> ObtenerAvisosGeneralesVigentes()
        {
            var response = await client.GetAsync("api/avisos/generales/vigentes");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoGeneralDetallesMaestroDTO>>();
            }
            return new List<AvisoGeneralDetallesMaestroDTO>();
        }

        public async Task<List<AvisoPersonalListaAlumnoDTO>> ObtenerAvisosPersonalesAlumno()
        {
            var response = await client.GetAsync($"api/avisos/personales/alumno");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoPersonalListaAlumnoDTO>>();
            }
            return new List<AvisoPersonalListaAlumnoDTO>();
        }

        public async Task CrearAvisoGeneral(CrearAvisoGeneralDto dto)
        {
            var response = await client.PostAsJsonAsync("api/avisos/general", dto);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar error
            }
        }

        public async Task CrearAvisoPersonal(CrearAvisoPersonalDto dto)
        {
            var response = await client.PostAsJsonAsync("api/avisos/personal", dto);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar error
            }
        }

        //public async Task MarcarAvisoPersonalComoLeido(int alumnoId, int avisoId)
        //{
        //    var response = await client.PostAsync($"api/avisos/personal/marcar-leido?alumnoId={alumnoId}&avisoId={avisoId}", null);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        // Manejar error
        //    }
        //}

        public async Task<List<AvisoGeneralListaAlumnoDTO>> ObtenerAvisosGeneralesPorAlumno()
        {
            var response = await client.GetAsync($"api/avisos/generales");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoGeneralListaAlumnoDTO>>();
            }
            return new List<AvisoGeneralListaAlumnoDTO>();
        }

        public async Task MarcarAvisosPersonalesLeidos(List<int> avisoIds, int alumnoId)
        {
            var response = await client.PutAsJsonAsync($"api/avisos/personal/marcarleidos", avisoIds);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar error
            }
        }
        public async Task<List<AvisoPersonalDetallesMaestroDTO>> ObtenerAvisosPersonalesPorAlumno(int alumnoId)
        {
            var response = await client.GetAsync($"api/avisos/personales/{alumnoId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoPersonalDetallesMaestroDTO>>();
            }
            return new List<AvisoPersonalDetallesMaestroDTO>();
        }

        public async Task<AvisoPersonalListaAlumnoDTO> ObtenerAvisoPersonalAlumno(int idAviso) { 
            var response = await client.GetAsync($"api/avisos/personal/alumno/{idAviso}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AvisoPersonalListaAlumnoDTO>();
            }
            return null;
        }

        public async Task<AvisoGeneralListaAlumnoDTO> ObtenerAvisoGeneralAlumno(int idAviso)
        {
            var response = await client.GetAsync($"api/avisos/general/alumno/{idAviso}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AvisoGeneralListaAlumnoDTO>();
            }
            return null;
        }

        public async Task MarcarAvisoLeido(int avisoId)
        {
            var response = await client.PutAsync($"api/avisos/marcarleido/{avisoId}",null);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar error
            }
        }

        public async Task CrearAlumno(AlumnoCreateDTO dto)
        {
            var response = await client.PostAsJsonAsync("api/alumnos", dto);
            if (!response.IsSuccessStatusCode)
            {
                // Manejar error
            }
        }
    }
}
