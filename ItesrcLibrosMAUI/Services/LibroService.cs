using ItesrcLibrosMAUI.Models.Dtos;
using ItesrcLibrosMAUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItesrcLibrosMAUI.Services
{
    public class LibroService
    {
        HttpClient client;
        Repositories.LibroRepository libroRepository = new();
        public LibroService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://libros.itesrc.net/")
            };

        }
        public async Task Agregar(LibroDto dto)
        {
            //var json=JsonSerializer.Serialize(dto);
            //var response= await client.PostAsync("api/libros",new StringContent(json,Encoding.UTF8,"application/json"));


            var response = await client.PostAsJsonAsync<LibroDto>("api/libros", dto);
            if (response.IsSuccessStatusCode)
            {
                await GetLibros();
            }
            else
            {
                var errores = await response.Content.ReadAsStringAsync();
                throw new Exception(errores);
            }
        }
        public event Action? DatosActualizados;
        public async Task GetLibros()
        {

            try
            {
                var fecha = Preferences.Get("UltimaFechaActualizacion", DateTime.MinValue);
                bool aviso = false;
                var response = await client.GetFromJsonAsync<List<LibroDto>>($"api/libros/{fecha:yyyy-MM-dd}/{fecha:HH}/{fecha:mm}");
                if (response != null)
                {

                    foreach (var libro in response)
                    {
                        var entidad = libroRepository.Get(libro.Id ?? 0);
                        if (entidad == null && libro.Eliminado == false)
                        {
                            entidad = new()
                            {
                                Id = libro.Id ?? 0,
                                Titulo = libro.Titulo,
                                Autor = libro.Autor,
                                Portada = libro.Portada
                            };
                            libroRepository.Insert(entidad);
                            aviso= true;
                        }
                        else
                        {
                            if (entidad != null)
                            {

                                if (libro.Eliminado)
                                {
                                    libroRepository.Delete(entidad);
                                    aviso = true;

                                }
                                else
                                {
                                    if(libro.Titulo!=entidad.Titulo||libro.Autor!=entidad.Autor||libro.Portada!=entidad.Portada)
                                    {
                                        aviso = true;
                                        libroRepository.Update(entidad);
                                    }
                                }
                            }
                        }


                    }
                    if (aviso)
                    {
                        _=MainThread.InvokeOnMainThreadAsync(() =>
                        {

                            DatosActualizados?.Invoke();
                        });
                    }
                    Preferences.Set("UltimaFechaActualizacion", response.Max(x => x.Fecha));
                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
