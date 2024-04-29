using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ItesrcLibrosMAUI.Models.Dtos;
using ItesrcLibrosMAUI.Models.Entities;
using ItesrcLibrosMAUI.Models.Validators;
using ItesrcLibrosMAUI.Repositories;
using ItesrcLibrosMAUI.Services;
using ItesrcLibrosMAUI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItesrcLibrosMAUI.ViewModels
{
    public partial class LibrosViewModel : ObservableObject
    {
        [ObservableProperty]
        private LibroDto? dto;
        LibroRepository libroRepository=new LibroRepository();
        public ObservableCollection<Libro> Libros { get; set; }=new ObservableCollection<Libro>();


        LibroValidator validator=new LibroValidator();
        LibroService Service=new();

        public LibrosViewModel()
        {
            ActualizarLibros();
            
        }

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo()
        {
            Dto=new LibroDto();
            Shell.Current.GoToAsync("//Agregar");
        }

        [RelayCommand]
        public void Cancelar()
        {
            Dto = null;
            Error = "";
            Shell.Current.GoToAsync("//Lista");
        }

        [RelayCommand]
        public async void Agregar()
        {
            try
            {


                if (Dto != null)
                {
                    var resultado = validator.Validate(Dto);
                    if (resultado.IsValid)
                    {
                        
                        await Service.Agregar(Dto);
                        ActualizarLibros();
                        Cancelar();
                    }
                    else
                    {
                        Error = string.Join("\n", resultado.Errors.Select(x => x.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        void ActualizarLibros()
        {
            Libros.Clear();
            foreach (var libro in libroRepository.GetAll())
            {
                Libros.Add(libro);
            }
        }
    }
}
