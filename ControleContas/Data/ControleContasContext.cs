using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ControleContas.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleContas.Data
{
    public class ControleContasContext : DbContext
    {
        public ControleContasContext(DbContextOptions<ControleContasContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Conta> Contas { get; set; }  // Adicionado para o novo modelo

    }

}
