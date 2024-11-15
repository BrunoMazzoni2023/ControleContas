using ControleContas.Models;
using ControleContas.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ControleContas.Controllers
{
    public class AuthController : Controller
    {
        private readonly ContasContext _context;
        private IEnumerable<Claim>? claims;

        public AuthController(ContasContext context)
        {
            _context = context;
        }

        // Ação para exibir a página de login
        public IActionResult Login() => View();

        public IActionResult Index()
        {
            return View();
        }

        // Ação para processar o login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario != null)
            {
                // Defina os claims para a identidade do usuário
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),  // Suponha que "usuario.Nome" seja o nome do usuário
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()), // Suponha que "usuario.Id" seja o ID único do usuário
            new Claim(ClaimTypes.Email, usuario.Email) // Opcional: adiciona o email do usuário
        };

                // Crie o ClaimsIdentity com os claims
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Defina propriedades de autenticação (opcional)
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true  // Define se o cookie de autenticação será persistente
                };

                // Faz login do usuário com cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                              new ClaimsPrincipal(claimsIdentity),
                                              authProperties);

                // Redireciona para a página inicial
                return RedirectToAction("Index", "Auth");
            }

            // Adiciona erro ao ModelState e redireciona para a página de acesso negado se o login falhar
            ModelState.AddModelError("", "Email ou senha inválidos.");
            return RedirectToAction("AccessDenied", "Auth");
            //return View();
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

        // Ação para exibir a página de registro
        public IActionResult Logout() => View();

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout(Usuario usuario)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
  
}
