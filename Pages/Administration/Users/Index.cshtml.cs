using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Administration.Users
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public IList<AppUser> students { get; set; }
        public IList<AppUser> teachers { get; set; }
        public IList<AppUser> admins { get; set; }
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            admins = _context.Utilisateurs.ToList();


        }
    }
}