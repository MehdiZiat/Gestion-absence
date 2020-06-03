using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Etudiant
{
    [Authorize(Roles=GestionRole.StudentUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Classe classe { get; set; }
        public IList<Seance> Seances { get; set; }
        public Seance seanceActuel { get; set; }
        public bool etatSeanceActuel { get; set; }

        public IList<SeanceAbsence> modelSeanceAbsence { get; set; }
        public AppUser User { get; set; }
        public async Task OnGetAsync()
        {
            User = await _userManager.GetUserAsync(HttpContext.User);
            var date = DateTime.Now;
            seanceActuel = _context.Seances
                 .Where(s => s.ClasseId == User.ClasseId)
                 .Where(s => s.date >= date && s.date <= date.AddHours(1))
                 .Include(s => s.Classe)
                 .Include(s => s.Matiere)
                 .Include(s => s.Professeur).FirstOrDefault();
            classe = _context.Classes.Find(User.ClasseId);


            Seances = await _context.Seances
                .Where(s => s.ClasseId == User.ClasseId)
                .Where(s => s.date.Day == date.Day)
                .Include(s => s.Classe)
                .Include(s => s.Matiere)
                .Include(s => s.Professeur)
                .ToListAsync();
            modelSeanceAbsence = new List<SeanceAbsence>();
            Absence absence = new Absence();
            SeanceAbsence Inter;
            foreach (var item in Seances)
            {
                Inter = new SeanceAbsence();
                absence = await _context.Absences
                    .Where(s => s.SeanceId == item.Id)
                    .Where(s => s.EtudiantId == User.Id)
                    .FirstOrDefaultAsync();
                Inter.seance = item;
                Inter.absence = absence;
                modelSeanceAbsence.Add(Inter);

                if(seanceActuel!=null && Inter.seance.Id == seanceActuel.Id)
                {
                    etatSeanceActuel = Inter.absence.statut;
                }
            }
        }
        public async Task<IActionResult> OnGetMarquerPresenceAsync(int id)
        {
            User = await _userManager.GetUserAsync(HttpContext.User);
            Absence abs = _context.Absences
                .Where(s=>s.EtudiantId==User.Id)
                .Where(s => s.SeanceId == id)
                .FirstOrDefault();
            if (abs != null)
            {
                abs.statut = false;
                abs.date = DateTime.Now;
                _context.Absences.Attach(abs);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./SeancesAvant");
        }
    }
}