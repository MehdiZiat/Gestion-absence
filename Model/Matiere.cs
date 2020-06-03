using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class Matiere
    {
        public int Id { get; set; }
        public string libele { get; set; }
        [ForeignKey("Filiere")]
        public int FiliereId { get; set; }
        public Filiere Filiere { get; set; }

    }
}
