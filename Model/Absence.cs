using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class Absence
    {
        public int Id { get; set; }
        public bool statut { get; set; }
        public DateTime date { get; set; }
        [ForeignKey("Seance")]
        public int SeanceId { get; set; }
        [ForeignKey("Etudiant")]
        public string EtudiantId { get; set; }
        public virtual Seance Seance { get; set; }
        public virtual AppUser Etudiant { get; set; }
    }
}
