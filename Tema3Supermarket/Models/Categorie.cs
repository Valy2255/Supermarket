using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public virtual ICollection<Produs> Produse { get; set; }
    }

}
