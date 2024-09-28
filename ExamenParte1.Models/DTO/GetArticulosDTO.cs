using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenParte1.Models.DTO
{
    public class GetArticulosDTO
    {
        public string CodigoSKU { get; set; }
        public string Descripcion { get; set; }
        public int Existencia { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal Total { get; set; }
        public bool GeneraImpuesto { get; set; }
    }
}
