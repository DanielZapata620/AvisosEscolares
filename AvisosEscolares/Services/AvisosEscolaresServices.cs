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

        public async Task<List<AlumnoDetallesListaDTO>> GetAlumnosByGrupo(int grupoId)
        {
            var response = await client.GetAsync($"api/alumnos/grupo/{grupoId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AlumnoDetallesListaDTO>>();
            }
            return new List<AlumnoDetallesListaDTO>();
        }
        public async Task<LoginResponseDTO?> Login(LoginDTO loginDto)
        {
            var response = await client.PostAsJsonAsync("api/auth/", loginDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDTO>();
            }
            return null;
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

        public async Task<List<AvisoPersonalDetallesMaestroDTO>> ObtenerAvisosPersonalesPorAlumno(int alumnoId)
        {
            var response = await client.GetAsync($"api/avisos/personales/alumno/{alumnoId}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<AvisoPersonalDetallesMaestroDTO>>();
            }
            return new List<AvisoPersonalDetallesMaestroDTO>();
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
