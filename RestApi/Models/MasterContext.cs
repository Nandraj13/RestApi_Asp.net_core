using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestApi.Models;

public partial class MasterContext : DbContext
{
    public MasterContext()
    {
    }

    public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Eid).HasName("PK__tmp_ms_x__D9509F6DF23C4914");

            entity.ToTable("employee");

            entity.Property(e => e.Eid).HasColumnName("eid");
            entity.Property(e => e.Eage).HasColumnName("eage");
            entity.Property(e => e.Edept)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("edept");
            entity.Property(e => e.Ename)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ename");
            entity.Property(e => e.Esalary).HasColumnName("esalary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
