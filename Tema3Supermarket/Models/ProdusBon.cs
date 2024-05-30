using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class ProdusBon
    {
        public int Id { get; set; }
        public int BonId { get; set; }
        public int ProdusId { get; set; }
        public int Cantitate { get; set; }
        public decimal Subtotal { get; set; }
        public virtual BonCasa BonCasa { get; set; }
        public virtual Produs Produs { get; set; }
    }

}
