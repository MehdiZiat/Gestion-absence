using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using miniProjet.Model;

namespace miniProjet.Pages.Administration
{
    [Authorize(Roles=GestionRole.AdminUser)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}