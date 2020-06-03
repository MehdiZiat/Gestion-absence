using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Etudiant
{
    [Authorize(Roles = GestionRole.StudentUser)]
    public class SeancesAvantModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SeancesAvantModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IList<Seance>seances { get; set; }
        public IList<SeanceAbsence> modeleSeanceAbsences { get; set; }
        public AppUser User { get; set; }
        public async Task OnGetAsync()
        {
            User = await _userManager.GetUserAsync(HttpContext.User);
            var date = DateTime.Now;
            seances = await _context.Seances
                .Where(s => s.ClasseId == User.ClasseId)
                .Where(s => s.date.Day < date.Day)
                .Include(s => s.Classe)
                .Include(s => s.Matiere)
                .Include(s => s.Professeur)
                .OrderBy(s=>s.date)
                .ToListAsync();
            modeleSeanceAbsences = new List<SeanceAbsence>();
            Absence absence = new Absence();
            SeanceAbsence Inter;
            foreach (var item in seances)
            {
                Inter = new SeanceAbsence();
                absence = await _context.Absences
                    .Where(s => s.SeanceId == item.Id)
                    .Where(s => s.EtudiantId == User.Id)
                    .FirstOrDefaultAsync();
                Inter.seance = item;
                Inter.absence = absence;
                modeleSeanceAbsences.Add(Inter);

               
            }
        }
    }
}
