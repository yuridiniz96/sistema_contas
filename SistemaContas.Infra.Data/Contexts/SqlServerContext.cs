using Microsoft.EntityFrameworkCore;
using SistemaContas.Domain.Entities;
using SistemaContas.Infra.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemaContas.Infra.Data.Contexts
{
    /// <summary>
    /// Classe de contexto do EntityFramework
    /// </summary>
    public class SqlServerContext : DbContext
    {
        //Método para configurar a connectionstring
        protected override void OnConfiguring
        (DbContextOptionsBuilder optionsBuilder)

        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BDSistemaContas;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        }
        //Método para adicionar cada classe de mapeamento
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaMapping());
        }
        //Propriedade DbSet para cada entidade
        public DbSet<Conta> Conta { get; set; }
    }
}