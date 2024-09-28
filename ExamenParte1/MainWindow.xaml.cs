using ExamenParte1.Services;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExamenParte1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await CargarArticulos();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Registro registro = new Registro();
            registro.Show();

            this.Close();
        }

        private async void refrescarGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            await CargarArticulos();
        }


        private async void EditarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button editarButton && editarButton.Tag is string codigoSKU)
            {
                EditarArticulo editar = new EditarArticulo(codigoSKU);
                editar.Show();

                this.Close();
            }
        }


        private async void EliminarArticulo_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button eliminarButton && eliminarButton.Tag is string codigoSKU)
            {
                var result = MessageBox.Show($"¿Estás seguro de que deseas eliminar el artículo con Código SKU: {codigoSKU}?",
                                             "Confirmación",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var articuloService = new ArticuloService();
                    (bool success, string response) = await articuloService.EliminarArticulo(codigoSKU); ;

                    if (success)
                    {
                        MessageBox.Show($"Artículo con Código SKU: {codigoSKU} eliminado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        await CargarArticulos();
                    }
                    else
                    {
                        MessageBox.Show($"Error al eliminar el artículo: {response}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }


        private async Task CargarArticulos()
        {
            var articuloService = new ArticuloService();
            var articulos = await articuloService.GetArticulos();
            articulosDataGrid.ItemsSource = articulos;
        }
    }
}