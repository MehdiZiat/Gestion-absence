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

namespace miniProjet.Pages.Professeur
{
    [Authorize(Roles = GestionRole.TeacherUser)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IList<Seance> SeancesOfThisWeek { get; set; }
        public IList<Seance> Seances { get; set; }
        public IList<Seance> FutureSeances { get; set; }
        public AppUser user { get; set; }
        public async Task OnGetAsync()
        {
            user = await _userManager.GetUserAsync(HttpContext.User);
            var date = DateTime.Now;
            Seances = await _context.Seances
                .Where(s => s.date.Day<date.Day)
                .Where(s => s.ProfesseurId == user.Id)
                .OrderBy(s=>s.date)
                .Include(s => s.Matiere)
                .Include(s => s.Classe)
                .Include(s => s.Professeur)
                .ToListAsync();
            SeancesOfThisWeek = await _context.Seances.Where(s => s.date.Day <= (date.Day + 7))
                .Where(s=>s.date.Day>=date.Day)
                .Where(s => s.ProfesseurId == user.Id)
                .OrderBy(s => s.date)
                .Include(s => s.Matiere)
                .Include(s => s.Classe)
                .Include(s => s.Professeur)
                .ToListAsync();
            FutureSeances = await _context.Seances
                .Where(s => s.date.Day >= (date.Day + 7))
                .Where(s => s.ProfesseurId == user.Id)
                .OrderBy(s => s.date)
                .Include(s => s.Matiere)
                .Include(s => s.Classe)
                .Include(s => s.Professeur)
                .ToListAsync();

        }
    }
}