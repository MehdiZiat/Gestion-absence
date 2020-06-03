using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class Filiere
    {
        public int Id { get; set; }
        public string libele { get; set; }
        public virtual ICollection<Matiere> Matieres { get; set; }
    }
}
