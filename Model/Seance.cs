using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class Seance
    {
        public int Id { get; set; }
        public DateTime date { get; set; }
        [ForeignKey("Professeur")]
        public string ProfesseurId { get; set; }
        [ForeignKey("Matiere")]
        public int MatiereId { get; set; }
        [ForeignKey("Classe")]
        public int ClasseId { get; set; }
        public virtual AppUser Professeur { get; set; }
        public virtual Classe Classe { get; set; }
        public virtual Matiere Matiere { get; set; }
    }
}
