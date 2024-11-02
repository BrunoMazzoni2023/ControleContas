using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ControleContas.Data;
using ControleContas.Models;

namespace ControleContas.Controllers
{
    public class ContasController : Controller
    {
        private readonly ControleContasContext _context;

        public ContasController(ControleContasContext context)
        {
            _context = context;
        }

        // GET: Contas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contas.ToListAsync());
        }

        // GET: Contas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contas = await _context.Contas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contas == null)
            {
                return NotFound();
            }

            return View(contas);
        }

        // GET: Contas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Conta conta)
        {
            if (ModelState.IsValid)
            {
                conta.DataCadastro = DateTime.Now; // Define a data de cadastro como agora
                                                   // Defina Vencimento conforme sua lógica, por exemplo, 30 dias a partir de agora
                conta.Vencimento = DateTime.Now.AddDays(30); // Exemplo
                _context.Contas.Add(conta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(conta);
        }


        // GET: Contas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contas = await _context.Contas.FindAsync(id);
            if (contas == null)
            {
                return NotFound();
            }
            return View(contas);
        }

        // POST: Contas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Valor,Parcela,DataCriacao,DataVencimento")] Conta contas)
        {
            if (id != contas.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContasExists(contas.Id))
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
            return View(contas);
        }

        // GET: Contas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contas = await _context.Contas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contas == null)
            {
                return NotFound();
            }

            return View(contas);
        }

        // POST: Contas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contas = await _context.Contas.FindAsync(id);
            if (contas != null)
            {
                _context.Contas.Remove(contas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContasExists(int id)
        {
            return _context.Contas.Any(e => e.Id == id);
        }
    }
}
