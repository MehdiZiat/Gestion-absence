using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using miniProjet.Model;

namespace miniProjet.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Absence> Absences { get; set; }
        public virtual DbSet<Classe> Classes { get; set; }
        public virtual DbSet<Matiere> Matieres { get; set; }
        public virtual DbSet<Filiere> Filieres { get; set; }
        public virtual DbSet<Seance> Seances { get; set; }
        public virtual DbSet<AppUser> Utilisateurs { get; set; }



    }
}
