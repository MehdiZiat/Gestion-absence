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
    public class IndexModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;

        public IndexModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }
        public string nameOfMatiere(int MatiereId)
        {
            var query = from s in _context.Matieres
                        where s.Id == MatiereId
                        select new
                        {
                            libele = s.libele
                        };
            return query.FirstOrDefault().libele;
        }
        public IList<Seance> Seance { get;set; }
        

        public async Task OnGetAsync()
        {
            Seance = await _context.Seances
                .OrderByDescending(s=>s.date)
                .Include(s=>s.Matiere)
                .Include(s=>s.Classe)
                .Include(s=>s.Professeur)
                .ToListAsync();
            
            
        }
    }
}
