using ExamenParte1.Models.DTO;
using ExamenParte1.Models.Entity;
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
    public partial class EditarArticulo : Window
    {
        private string _codigoSKU;

        public EditarArticulo(string codigoSKU)
        {
            InitializeComponent();
            _codigoSKU = codigoSKU;

        }

        private async void EditarArticulo_Loaded(object sender, RoutedEventArgs e)
        {
            var articuloService = new ArticuloService();
            (GetArticulosDTO articulo, string response) = await articuloService.GetArticulo(_codigoSKU);
            txtCodigoSKU.Text = articulo.CodigoSKU;
            txtDescripcion.Text = articulo.Descripcion;
            txtExistencia.Text = articulo.Existencia.ToString();
            txtPrecioUnitario.Text = articulo.PrecioUnitario.ToString();
            cbGeneraImpuesto.SelectedItem = articulo.GeneraImpuesto ? "Si" : "No";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();

            this.Close();
        }

        private async void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            AgregarArticuloDTO addNewArticulo = new AgregarArticuloDTO();
            addNewArticulo.CodigoSKU = txtCodigoSKU.Text;
            addNewArticulo.Descripcion = txtDescripcion.Text;
            addNewArticulo.Existencia = txtExistencia.Text;
            addNewArticulo.PrecioUnitario = txtPrecioUnitario.Text;
            addNewArticulo.GeneraImpuesto = cbGeneraImpuesto.Text == "Si" ? true : false;

            var articuloService = new ArticuloService();
            (bool success, string response) = await articuloService.EditarArticulo(addNewArticulo);
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
