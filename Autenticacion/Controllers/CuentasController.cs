using Autenticacion.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Autenticacion.Controllers
{
    public class CuentasController : Controller
    {
        //inyeccion de dependencias
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registro(int id)
        {
            RegistroVM registroVM = new RegistroVM();
            return View(registroVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroVM registroVM)
        {
            if (ModelState.IsValid)
            {
                //PASAR LOS CAMPOS DE LA BD
                var usuario = new AppUsuarios
                {
                    UserName = registroVM.Email,
                    Email = registroVM.Email,
                    Nombre = registroVM.Nombre,
                    Url = registroVM.Url,
                    CodigoPais = registroVM.CodigoPais,
                    Telefono = registroVM.Telefono,
                    Pais = registroVM.Pais,
                    Ciudad = registroVM.Ciudad,
                    Direccion = registroVM.Direccion,
                    FechaNacimineto = registroVM.FechaNacimiento,
                    Estado = registroVM.Estado
                };
                //Crea el usuario en la BD con una contraseña
                var resultado = await _userManager.CreateAsync(usuario, registroVM.Password);

                //Validar si se inserta correctamente
                if (resultado.Succeeded)
                {
                    //el usuario inicia sesion, se crea la cookie
                    //y si el usuario cierra el navegador no continua autenticacion
                    await _signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                ValidarErrores(resultado);
            }
            return View(registroVM);
        }

        //manejador de errores
        private void ValidarErrores(IdentityResult resultado)
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        //metodo mostrar formulario de acceso
        [HttpGet]
        public IActionResult Acceso()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Acceso(AccesoVM accesoVM)
        {
            //inicia sesion en Email, passwrod, bool, Rememberme
            //y lockoutOnFailure: false si la cuenta de usuario debe bloquearse por intentos fallidos
            if (ModelState.IsValid)
            {
                var resultado = await _signInManager.PasswordSignInAsync(accesoVM.Email,
                    accesoVM.Password,
                    accesoVM.RememberMe,
                    lockoutOnFailure: false);

                if (resultado.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Acceso invalido");
                    return View(accesoVM);
                }
            }
            return View(accesoVM);
        }
        //Salir o cerrar sesion de la aplicacion(logout)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalirAplicacion() { 
        //SALE APLICACIOn Y DESTRUYE LAS COOKIES
        await _signInManager.SignOutAsync();
            //REDIRECCION A LA PAGINA DE INICIO
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
