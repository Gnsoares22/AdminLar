using PI_4º_Semestre.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace AdminLar.Models
{
    public class Context : DbContext
    {

        
        public Context():base("AdminLar")
        {


            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());

        }


        //para fazer as migrações

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Sindico> Sindicos { get; set; }

        public DbSet<Condomino> Condominos { get; set; }

        public DbSet<Porteiro> Porteiros { get; set; }

        public DbSet<Administrador> Administradores { get; set; }

        public DbSet<Visitante> Visitantes { get; set; }

        public DbSet<Aviso> Avisos { get; set; }

        public DbSet<AreaLazer> AreaLazer { get; set; }

        public DbSet<Apartamento> Apartamento { get; set; }

        public DbSet<Atas> Ata { get; set; }

        public DbSet<Encomendas> Encomenda { get; set; }

        public DbSet<Inadimplencia> Inadimplecias { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<Ocorrencias> Ocorrencias { get; set; }

        public DbSet<Balancetes> Balancetes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}