using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class Oferta
    {
        public int Id { get; set; }
        public string Motiv { get; set; }
        public int ProdusId { get; set; }
        public decimal ProcentReducere { get; set; }
        public DateTime DataInceput { get; set; }
        public DateTime DataSfarsit { get; set; }
        public virtual Produs Produs { get; set; }
    }

}
