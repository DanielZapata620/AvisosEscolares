using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AvisosEscolaresApi.Models.Entities;

public partial class AvisosescolaresContext : DbContext
{
    public AvisosescolaresContext()
    {
    }

    public AvisosescolaresContext(DbContextOptions<AvisosescolaresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Aviso> Aviso { get; set; }

    public virtual DbSet<Avisoalumnoestado> Avisoalumnoestado { get; set; }

    public virtual DbSet<Avisogeneral> Avisogeneral { get; set; }

    public virtual DbSet<Avisopersonal> Avisopersonal { get; set; }

    public virtual DbSet<Estado> Estado { get; set; }

    public virtual DbSet<Grupo> Grupo { get; set; }

    public virtual DbSet<Maestro> Maestro { get; set; }

    public virtual DbSet<Tipoaviso> Tipoaviso { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.GrupoId, "GrupoId");

            entity.HasIndex(e => e.Usuario, "Usuario").IsUnique();

            entity.Property(e => e.Contrasena).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(150);
            entity.Property(e => e.Usuario).HasMaxLength(50);

            entity.HasOne(d => d.Grupo).WithMany(p => p.Alumno)
                .HasForeignKey(d => d.GrupoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("alumno_ibfk_1");
        });

        modelBuilder.Entity<Aviso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aviso");

            entity.HasIndex(e => e.TipoAvisoId, "TipoAvisoId");

            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasColumnType("text");
            entity.Property(e => e.Titulo).HasMaxLength(200);

            entity.HasOne(d => d.TipoAviso).WithMany(p => p.Aviso)
                .HasForeignKey(d => d.TipoAvisoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("aviso_ibfk_1");
        });

        modelBuilder.Entity<Avisoalumnoestado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("avisoalumnoestado");

            entity.HasIndex(e => e.AlumnoId, "AlumnoId");

            entity.HasIndex(e => e.AvisoId, "AvisoId");

            entity.HasIndex(e => e.EstadoId, "EstadoId");

            entity.Property(e => e.FechaLeido).HasColumnType("datetime");

            entity.HasOne(d => d.Alumno).WithMany(p => p.Avisoalumnoestado)
                .HasForeignKey(d => d.AlumnoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisoalumnoestado_ibfk_2");

            entity.HasOne(d => d.Aviso).WithMany(p => p.Avisoalumnoestado)
                .HasForeignKey(d => d.AvisoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisoalumnoestado_ibfk_1");

            entity.HasOne(d => d.Estado).WithMany(p => p.Avisoalumnoestado)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisoalumnoestado_ibfk_3");
        });

        modelBuilder.Entity<Avisogeneral>(entity =>
        {
            entity.HasKey(e => e.AvisoId).HasName("PRIMARY");

            entity.ToTable("avisogeneral");

            entity.Property(e => e.AvisoId).ValueGeneratedNever();
            entity.Property(e => e.FechaCaducidad).HasColumnType("datetime");

            entity.HasOne(d => d.Aviso).WithOne(p => p.Avisogeneral)
                .HasForeignKey<Avisogeneral>(d => d.AvisoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisogeneral_ibfk_1");
        });

        modelBuilder.Entity<Avisopersonal>(entity =>
        {
            entity.HasKey(e => e.AvisoId).HasName("PRIMARY");

            entity.ToTable("avisopersonal");

            entity.HasIndex(e => e.MaestroId, "MaestroId");

            entity.Property(e => e.AvisoId).ValueGeneratedNever();
            entity.Property(e => e.Eliminado)
                .HasDefaultValueSql("b'0'")
                .HasColumnType("bit(1)");

            entity.HasOne(d => d.Aviso).WithOne(p => p.Avisopersonal)
                .HasForeignKey<Avisopersonal>(d => d.AvisoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisopersonal_ibfk_1");

            entity.HasOne(d => d.Maestro).WithMany(p => p.Avisopersonal)
                .HasForeignKey(d => d.MaestroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("avisopersonal_ibfk_2");
        });

        modelBuilder.Entity<Estado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("estado");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Grupo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("grupo");

            entity.HasIndex(e => e.MaestroId, "MaestroId");

            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasOne(d => d.Maestro).WithMany(p => p.Grupo)
                .HasForeignKey(d => d.MaestroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("grupo_ibfk_1");
        });

        modelBuilder.Entity<Maestro>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("maestro");

            entity.HasIndex(e => e.ClaveAcceso, "ClaveAcceso").IsUnique();

            entity.Property(e => e.ClaveAcceso).HasMaxLength(50);
            entity.Property(e => e.Contrasena).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(150);
        });

        modelBuilder.Entity<Tipoaviso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tipoaviso");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
