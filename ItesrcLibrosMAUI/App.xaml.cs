using ItesrcLibrosMAUI.Services;

namespace ItesrcLibrosMAUI
{
    public partial class App : Application
    {
        public static LibroService LibroService { get; set; } = new LibroService();
        public App()
        {
            InitializeComponent();

            Thread thread=new Thread(Sincronizador) { IsBackground = true };
            thread.Start();
            MainPage = new AppShell();
        }

       async void Sincronizador()
        {
            while (true)
            {

                await LibroService.GetLibros();
                Thread.Sleep(TimeSpan.FromSeconds(15));
            }
        }
    }
}
