using Microsoft.AspNetCore.Identity;

namespace Autenticacion.Models
{
    public class AppUsuarios : IdentityUser
    {
        public string Nombre { get; set; }
        public string Url { get; set; }
        public int CodigoPais { get; set; }
        public string Telefono { get; set; }
        public string Pais { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimineto { get; set; }
        public bool Estado { get; set; }
    }
}
