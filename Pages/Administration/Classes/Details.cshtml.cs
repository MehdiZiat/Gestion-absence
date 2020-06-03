using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Classes
{
    public class DetailsModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;
        

        public DetailsModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Classe Classe { get; set; }
        public IEnumerable<AppUser> EtudiantsActuel { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EtudiantsActuel = await _context.Utilisateurs.Where(s => s.ClasseId == id).OrderBy(s=>s.lastName).ToListAsync();
            Classe = await _context.Classes.FirstOrDefaultAsync(m => m.Id == id);

            if (Classe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
