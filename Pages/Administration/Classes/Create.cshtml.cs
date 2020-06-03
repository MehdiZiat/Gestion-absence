using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Classes
{
    public class CreateModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;

        public CreateModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Classe Classe { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Classes.Add(Classe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
