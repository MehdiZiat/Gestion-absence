using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class EtudiantAbsence
    {
        public AppUser Etudiant{ get; set; }
        public Absence absence { get; set; }
    }
}
