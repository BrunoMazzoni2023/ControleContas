using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleContas.Data;
using ControleContas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleContas.Services
{
    public class ContaService
    {
        private readonly ContasContext _context;

        public ContaService(ContasContext context)
        {
            _context = context;
        }

        public async Task VerificarContasVencidas()
        {
            var contas = await _context.Conta
                .Where(c => c.Vencimento < DateTime.Now && c.StatusConta != StatusConta.Pago)
                .ToListAsync();

            foreach (var conta in contas)
            {
                conta.StatusConta = StatusConta.Vencido;
            }

            await _context.SaveChangesAsync();
        }
    }
}
