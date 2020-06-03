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
    public class IndexModel : PageModel
    {
        private readonly miniProjet.Data.ApplicationDbContext _context;

        public IndexModel(miniProjet.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Classe> Classe { get;set; }

        public async Task OnGetAsync()
        {
            Classe = await _context.Classes.ToListAsync();
        }
    }
}
