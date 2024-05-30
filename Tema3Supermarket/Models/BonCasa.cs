using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class BonCasa
    {
        public int Id { get; set; }
        public DateTime DataEliberare { get; set; }
        public int CasierId { get; set; }
        public decimal SumaIncasata { get; set; }
        public virtual Utilizator Casier { get; set; }
        public virtual ICollection<ProdusBon> ProduseBon { get; set; }
    }

}
