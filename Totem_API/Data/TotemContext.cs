using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Totem_API.Models;

namespace Totem_API.Data
{
    public partial class TotemContext : DbContext
    {
        public TotemContext()
        {
        }

        public TotemContext(DbContextOptions<TotemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Locacion> Locacions { get; set; }
        public virtual DbSet<Publicidad> Publicidads { get; set; }
        public virtual DbSet<Totem> Totems { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Archivo> Archivos { get; set; }
        public virtual DbSet<Ubicacion> Ubicacion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost,8433;Database=Totem;Trusted_Connection=false;User Id=sa;Password=TotemUnivalle23;Encrypt=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la tabla Locacion
            modelBuilder.Entity<Locacion>(entity =>
            {
                entity.HasKey(e => e.IdLocacion);

                entity.ToTable("Locacion");

                entity.Property(e => e.IdLocacion).HasColumnName("ID_locacion");
                entity.Property(e => e.Descripcion).HasMaxLength(250).IsUnicode(false).HasColumnName("descripcion");
                entity.Property(e => e.IdTotem).HasColumnName("id_totem");
                entity.Property(e => e.Keywords).HasColumnType("text").HasColumnName("keywords");
                entity.Property(e => e.Nombre).HasMaxLength(150).IsUnicode(false).HasColumnName("nombre");
                entity.Property(e => e.UrlCarruselImagenes).HasColumnType("text").HasColumnName("urlCarruselImagenes");
                entity.Property(e => e.UrlMapa).IsUnicode(false).HasColumnName("urlMapa");

                entity.HasOne(d => d.IdTotemNavigation)
                    .WithMany(p => p.Locacions)
                    .HasForeignKey(d => d.IdTotem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Locacion_Totem");
            });

            // Configuración de la tabla Publicidad
            modelBuilder.Entity<Publicidad>(entity =>
            {
                entity.HasKey(e => e.IdPublicidad);

                entity.ToTable("Publicidad");

                entity.Property(e => e.IdPublicidad).HasColumnName("ID_publicidad");
                entity.Property(e => e.FechaFin).HasColumnType("datetime").HasColumnName("fechaFin");
                entity.Property(e => e.FechaInicio).HasColumnType("datetime").HasColumnName("fechaInicio");
                entity.Property(e => e.IdTotem).HasColumnName("id_totem");
                entity.Property(e => e.UrlPublicidad).IsUnicode(false).HasColumnName("urlPublicidad");

                entity.HasOne(d => d.IdTotemNavigation)
                    .WithMany(p => p.Publicidads)
                    .HasForeignKey(d => d.IdTotem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Publicidad_Totem");
            });

            // Configuración de la tabla Totem
            modelBuilder.Entity<Totem>(entity =>
            {
                entity.HasKey(e => e.IdTotem);

                entity.ToTable("Totem");

                entity.Property(e => e.IdTotem).ValueGeneratedOnAdd().HasColumnName("ID_totem");
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.Nombre).HasMaxLength(150).IsUnicode(false).HasColumnName("nombre");
                entity.Property(e => e.NumeroPlantilla).HasColumnName("numeroPlantilla");
                entity.Property(e => e.UrlLogo).IsUnicode(false).HasColumnName("urlLogo");

                entity.HasOne(d => d.IdTotemNavigation)
                    .WithOne(p => p.Totem)
                    .HasForeignKey<Totem>(d => d.IdTotem)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Totem_Usuario");

                entity.HasMany(d => d.Ubicaciones)
                    .WithOne(p => p.Totem)
                    .HasForeignKey(p => p.IdTotem)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Totem_Ubicaciones");
            });

            // Configuración de la tabla Ubicacion
            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.ToTable("Ubicacion");

                entity.Property(e => e.Activo).HasDefaultValueSql("((1))");
                entity.Property(e => e.Direccion).HasMaxLength(250);
                entity.Property(e => e.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
                entity.Property(e => e.Nombre).HasMaxLength(100);

                entity.HasOne(d => d.Totem)
                    .WithMany(p => p.Ubicaciones)
                    .HasForeignKey(d => d.IdTotem)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de la tabla Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
                entity.Property(e => e.Apellido).HasMaxLength(75).IsUnicode(false).HasColumnName("apellido");
                entity.Property(e => e.Email).HasMaxLength(75).IsUnicode(false).HasColumnName("email");
                entity.Property(e => e.Institucion).HasMaxLength(150).IsUnicode(false).HasColumnName("institucion");
                entity.Property(e => e.Nombre).HasMaxLength(50).IsUnicode(false).HasColumnName("nombre");
                entity.Property(e => e.Password).HasMaxLength(150).IsUnicode(false).HasColumnName("password");
                entity.Property(e => e.Rol).HasColumnName("rol");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
