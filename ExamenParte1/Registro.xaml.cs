using ExamenParte1.Models.DTO;
using ExamenParte1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ExamenParte1
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Window
    {
        public Registro()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }

        private async void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtPrecioUnitario.Text, out int precioUnitario) || precioUnitario <= 0 ||
            !int.TryParse(txtExistencia.Text, out int existencia) || existencia <= 0)
            {
                MessageBox.Show("Por favor, ingrese números válidos y positivos en ambos campos.", "Error de Validación", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(txtPrecioUnitario.Text))
            {
                MessageBox.Show("Precio Unitario Requerido", "Error", MessageBoxButton.OK);
            }


            AgregarArticuloDTO addNewArticulo = new AgregarArticuloDTO();
            addNewArticulo.CodigoSKU = txtCodigoSKU.Text;
            addNewArticulo.Descripcion = txtDescripcion.Text;
            addNewArticulo.Existencia = txtExistencia.Text;
            addNewArticulo.PrecioUnitario = txtPrecioUnitario.Text;
            addNewArticulo.GeneraImpuesto = cbGeneraImpuesto.Text == "Si" ? true : false;

            var articuloService = new ArticuloService();
            (bool success, string response) = await articuloService.AgregarArticulo(addNewArticulo);
            if (success)
            {
                MessageBox.Show(response, "Exito", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show(response, "Error", MessageBoxButton.OK);
            }

        }
    }
}
