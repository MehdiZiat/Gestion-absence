using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Classes
{
    public class EditModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public IEnumerable<AppUser> EtudiantsSansClasse { get; set; }
        public IEnumerable<AppUser> EtudiantsActuel { get; set; }


        public EditModel(miniProjet.Data.ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Classe Classe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            EtudiantsSansClasse = await _context.Utilisateurs.Where(s => s.ClasseId == 0).Where(s => s.role == "Etudiant").ToListAsync();
            EtudiantsActuel = await _context.Utilisateurs.Where(s => s.ClasseId == id).Where(s => s.role == "Etudiant").ToListAsync();
            Classe = await _context.Classes.FirstOrDefaultAsync(m => m.Id == id);

            if (Classe == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Classe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClasseExists(Classe.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        public async Task<IActionResult> OnGetAddStudentAsync(string id, int idclasse)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            user.ClasseId = idclasse;
            await _userManager.UpdateAsync(user);
            return RedirectToPage("./Edit", new { id = idclasse });
        }
        public async Task<IActionResult> OnGetRemoveStudentAsync(string id, int idclasse)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            user.ClasseId = 0;
            await _userManager.UpdateAsync(user);
            return RedirectToPage("./Edit", new { id = idclasse });
        }
        private bool ClasseExists(int id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }
    }
}
