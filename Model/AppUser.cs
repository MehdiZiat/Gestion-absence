using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace miniProjet.Model
{
    public class AppUser:IdentityUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string role { get; set; }
        [ForeignKey("Classe")]
        public int ClasseId { get; set; }
        public Classe Classe{ get; set; }
        public ICollection<Seance> Seances { get; set; }

    }
}
