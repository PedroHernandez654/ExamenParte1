using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenParte1.Models.DTO
{
    public class AgregarArticuloDTO
    {
        public string CodigoSKU { get; set; }
        public string Descripcion { get; set; }
        public string Existencia { get; set; }
        public string PrecioUnitario { get; set; }
        public bool GeneraImpuesto { get; set; }
    }
}
