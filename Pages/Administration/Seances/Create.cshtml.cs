using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Seances
{
    public class CreateModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;
        public IList<Matiere> matieres { get; set; }
        public IList<Classe> classes { get; set; }
        public IList<AppUser> professeurs { get; set; }
        public SelectList listMatiers { get; set; }
        public SelectList listClasses { get; set; }
        public SelectList listProfs { get; set; }

        public IList<AppUser> Etudiants { get; set; }

        public CreateModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            matieres = _context.Matieres.ToList();
            classes = _context.Classes.ToList();
            professeurs = _context.Utilisateurs.Where(s=>s.role=="Professeur").ToList();
            listMatiers = new SelectList(matieres, "Id", "libele");
            listClasses = new SelectList(classes, "Id", "libele");
            listProfs = new SelectList(professeurs, "Id", "UserName");

            return Page();
        }

        [BindProperty]
        public Seance Seance { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Seances.Add(Seance);
            await _context.SaveChangesAsync();

            Etudiants = await _context.Utilisateurs.Where(s => s.ClasseId == Seance.ClasseId).ToListAsync();

            Absence abs;

            foreach(var item in Etudiants)
            {
                abs = new Absence();
                abs.EtudiantId = item.Id;
                abs.SeanceId = Seance.Id;
                abs.statut = true;
                _context.Absences.Add(abs);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
