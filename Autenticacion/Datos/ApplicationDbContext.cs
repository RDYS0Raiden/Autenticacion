using Autenticacion.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Autenticacion.Datos
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opciones)
            : base(opciones)
        {
        }

        //AGREGAR EL MODELO
        public DbSet<AppUsuarios> AppUsuarios { get; set; }
    }
}
