using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVC_Practica01NETCore.Models;

public partial class Practica1Ds2Context : DbContext
{
    public Practica1Ds2Context()
    {
    }

    public Practica1Ds2Context(DbContextOptions<Practica1Ds2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Persona> Personas { get; set; }

   // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
 //       => optionsBuilder.UseSqlServer("Server=DESKTOP-7BSEMKS\\SQLEXPRESS;Database=Practica1_DS2;User ID=N3kr;Password=@prkal3;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PERSONA__3214EC072D1BB158");

            entity.ToTable("PERSONA");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Apellido)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.CarnetExtranjero)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Dni)
                .HasMaxLength(8)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("DNI");
            entity.Property(e => e.FechaNacimiento).HasColumnType("date");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.Pasaporte)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
