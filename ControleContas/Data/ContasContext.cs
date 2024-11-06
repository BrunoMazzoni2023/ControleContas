using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ControleContas.Models;

namespace ControleContas.Data
{
    public class ContasContext : DbContext
    {
        public ContasContext (DbContextOptions<ContasContext> options)
            : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ControleContas.Models.Conta> Conta { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conta>()
                .Property(c => c.StatusConta)
                .HasConversion<string>(); // Converte o enum para string
        }
    }
}
