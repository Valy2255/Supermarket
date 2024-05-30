using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class Produs
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string CodBare { get; set; }
        public int CategorieId { get; set; }
        public int ProducatorId { get; set; }
        public virtual Categorie Categorie { get; set; }
        public virtual Producator Producator { get; set; }
        public virtual ICollection<Stoc> Stocuri { get; set; }
        public virtual ICollection<ProdusBon> ProduseBon { get; set; }
        public virtual ICollection<Oferta> Oferte { get; set; }
    }

}
