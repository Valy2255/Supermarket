using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tema3Supermarket.Models
{
    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Parola { get; set; }
        public string TipUtilizator { get; set; }  
        public virtual ICollection<BonCasa> BonuriCasa { get; set; }
    }

}
