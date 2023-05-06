using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FINALIDS_1220019.Models;

public partial class FinalIdsContext : DbContext
{
    public FinalIdsContext()
    {
    }

    public FinalIdsContext(DbContextOptions<FinalIdsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidato> Candidatos { get; set; }

    public virtual DbSet<Estadistica> Estadisticas { get; set; }

    public virtual DbSet<Voto> Votos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)

        {

            IConfigurationRoot configuration = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())

                        .AddJsonFile("appsettings.json")

                        .Build();

            var connectionString = configuration.GetConnectionString("ConexionBD");

            optionsBuilder.UseMySQL(connectionString);

        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("candidato");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dpi)
                .HasMaxLength(255)
                .HasColumnName("DPI");
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .HasColumnName("nombre");
            entity.Property(e => e.Partido).HasMaxLength(255);
        });

        modelBuilder.Entity<Estadistica>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estadisticas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasMaxLength(255);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Usuario1).HasMaxLength(255)
                .HasColumnName("usuario");
            entity.Property(e => e.Contraseña).HasMaxLength(255);
        });

        modelBuilder.Entity<Voto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("voto");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CandidatoVotado).HasMaxLength(255);
            entity.Property(e => e.Dpi)
                .HasMaxLength(20)
                .HasColumnName("DPI");
            entity.Property(e => e.FechaVoto).HasColumnType("date");
            entity.Property(e => e.Iporigen)
                .HasMaxLength(50)
                .HasColumnName("IPOrigen");
            entity.Property(e => e.NombreVotante).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
