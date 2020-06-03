using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration
{
    [Authorize(Roles = GestionRole.AdminUser)]
    public class TauxModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;

        public TauxModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Matiere> Matiere { get; set; }
        public IList<MatiereAbsence> MatiereAbsences { get; set; }
        public IList<ClasseAbsence> ClasseAbsences { get; set; }

        public async Task OnGetAsync()
        {
            IList<Seance> seances;
            MatiereAbsence MA;
            ClasseAbsence ca;
            int T, present, total;
            Matiere = await _context.Matieres
                .Include(m => m.Filiere).ToListAsync();
            MatiereAbsences = new List<MatiereAbsence>();
            ClasseAbsences = new List<ClasseAbsence>();
            foreach (var item in Matiere)
            {
                MA = new MatiereAbsence();
                T = 0;
                present = 0;
                total = 0;
                seances = new List<Seance>();
                seances = _context.Seances.Where(s => s.MatiereId == item.Id).ToList();
                if (seances != null)
                {
                    foreach (var seance in seances)
                    {
                        present += _context.Absences.Where(s => s.SeanceId == seance.Id).Where(s => s.statut == false).Count();
                        total += _context.Absences.Where(s => s.SeanceId == seance.Id).Count();
                    }
                }
                if (total != 0)
                {
                    T = 100 * present / total;
                }
                else
                {
                    T = -1;
                }
                MA.matiere = item;
                MA.taux = T;
                MatiereAbsences.Add(MA);
            }

            var classes = _context.Classes.ToList();
            
            foreach (var c in classes)
            {
                T = 0;
                present = 0;
                total = 0;
                ca = new ClasseAbsence();
                present += _context.Absences.Where(s => s.Etudiant.ClasseId == c.Id).Where(s => s.statut == false).Count();
                total += _context.Absences.Where(s => s.Etudiant.ClasseId == c.Id).Count();
                if (total != 0)
                {
                    T = 100 * present / total;
                }
                else
                {
                    T = -1;
                }
                ca.classe = c;
                ca.taux = T;
                ClasseAbsences.Add(ca);
            }

        }
    }
}

