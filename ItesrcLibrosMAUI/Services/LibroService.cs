using ItesrcLibrosMAUI.Models.Dtos;
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

        public async Task GetLibros()
        {

        }
    }
}
