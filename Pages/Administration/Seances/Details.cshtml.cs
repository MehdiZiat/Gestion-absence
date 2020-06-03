using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Seances
{
    public class DetailsModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;

        public DetailsModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Seance Seance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Seance = await _context.Seances
                .Include(s => s.Classe)
                .Include(s => s.Matiere)
                .Include(s => s.Professeur).FirstOrDefaultAsync(m => m.Id == id);

            if (Seance == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
