using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Pages.Professeur
{
    public class listModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public listModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public IList<AppUser> Etudiants { get; set; }
        public IList<EtudiantAbsence> model { get; set; }
        public Classe classe { get; set; }
        public Seance seance { get; set; }
        public void OnGet(int id)
        {
            model = new List<EtudiantAbsence>();
            EtudiantAbsence ea;
            seance = _context.Seances.Include(s => s.Matiere).Where(s => s.Id == id).FirstOrDefault();
            Etudiants = _context.Utilisateurs.OrderBy(s=>s.lastName).Where(s => s.ClasseId == seance.ClasseId).ToList();
            classe = _context.Classes.Find(seance.ClasseId);
            foreach(var item in Etudiants)
            {
                ea = new EtudiantAbsence();
                ea.Etudiant = item;
                ea.absence = _context.Absences.Where(s => s.EtudiantId == item.Id).Where(s => s.SeanceId == id).FirstOrDefault();
                
                model.Add(ea);

            }
        }
    }
}