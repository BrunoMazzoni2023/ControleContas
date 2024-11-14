using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleContas.Data;
using ControleContas.Models;
using Microsoft.AspNetCore.Authorization;



namespace ControleContas.Controllers
{
    [Authorize]
    public class ContasController : Controller
    {
        private readonly ContasContext _context;

        public ContasController(ContasContext context)
        {
            _context = context;
        }

        // GET: Contas
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var contas = await _context.Conta.ToListAsync();

            // Atualiza o status das contas com base na data de vencimento
            foreach (var conta in contas)
            {
                if (conta.Vencimento < DateTime.Now && conta.StatusConta == StatusConta.Pendente)
                {
                    conta.StatusConta = StatusConta.Vencido;
                    _context.Update(conta);
                }
            }

            await _context.SaveChangesAsync();
            return View(contas);
        }

        // GET: Contas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = await _context.Conta.FirstOrDefaultAsync(m => m.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            return View(conta);
        }

        // GET: Contas/Create
     
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Valor,DataCadastro,Vencimento,Parcela,StatusConta")] Conta conta)
        {
            if (ModelState.IsValid)
            {
                conta.StatusConta = StatusConta.Pendente; // Define o status como Pendente ao criar
                _context.Add(conta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(conta);
        }

        // Método para verificar e atualizar o status das contas vencidas
        public async Task<IActionResult> VerificarVencimentos()
        {
            var contas = await _context.Conta
                .Where(c => c.Vencimento < DateTime.Now && c.StatusConta != StatusConta.Pago)
                .ToListAsync();

            foreach (var conta in contas)
            {
                conta.StatusConta = StatusConta.Vencido;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Contas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = await _context.Conta.FindAsync(id);
            if (conta == null)
            {
                return NotFound();
            }
            return View(conta);
        }

        // POST: Contas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,DataCadastro,Vencimento,Parcela,StatusConta")] Conta conta)
        {
            if (id != conta.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(conta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContaExists(conta.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(conta);
        }

        // GET: Contas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conta = await _context.Conta.FirstOrDefaultAsync(m => m.Id == id);
            if (conta == null)
            {
                return NotFound();
            }

            return View(conta);
        }

        // POST: Contas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var conta = await _context.Conta.FindAsync(id);
            if (conta != null)
            {
                _context.Conta.Remove(conta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContaExists(int id)
        {
            return _context.Conta.Any(e => e.Id == id);
        }
    }
}
