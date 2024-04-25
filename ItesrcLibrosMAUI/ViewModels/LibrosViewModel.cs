using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ItesrcLibrosMAUI.Models.Dtos;
using ItesrcLibrosMAUI.Models.Validators;
using ItesrcLibrosMAUI.Services;
using ItesrcLibrosMAUI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItesrcLibrosMAUI.ViewModels
{
    public partial class LibrosViewModel : ObservableObject
    {
        [ObservableProperty]
        private LibroDto? dto;


        LibroValidator validator=new LibroValidator();
        LibroService Service=new();

        [ObservableProperty]
        private string error = "";

        [RelayCommand]
        public void Nuevo()
        {
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
    }
}
