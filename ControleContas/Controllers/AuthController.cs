using ControleContas.Models;
using ControleContas.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ControleContas.Controllers
{
    public class AuthController : Controller
    {
        private readonly ControleContasContext _context;

        public AuthController(ControleContasContext context)
        {
            _context = context;
        }

        // Ação para exibir a página de login
        public IActionResult Login() => View();

        // Ação para processar o login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                // Lógica para autenticar o usuário
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email ou senha inválidos.");
            return View();
        }

        // Ação para exibir a página de registro
        public IActionResult Register() => View();

        // Ação para processar o registro
        [HttpPost]
        public async Task<IActionResult> Register(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login));
            }
            return View(usuario);
        }
    }
}
