using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class Stoc
    {
        public int Id { get; set; }
        public int ProdusId { get; set; }
        public int Cantitate { get; set; }
        public string UnitateMasura { get; set; }
        public DateTime DataAprovizionare { get; set; }
        public DateTime? DataExpirare { get; set; }
        public decimal PretAchizitie { get; set; }
        public decimal PretVanzare { get; set; }
        public virtual Produs Produs { get; set; }
    }

}
