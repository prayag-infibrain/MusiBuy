using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MusiBuy.Admin.DB;

public partial class MusiBuyDB_Connection : DbContext
{
    public MusiBuyDB_Connection()
    {
    }

    public MusiBuyDB_Connection(DbContextOptions<MusiBuyDB_Connection> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminUser> AdminUsers { get; set; }

    public virtual DbSet<CommonSetting> CommonSettings { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Creatore> Creatores { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Enum> Enums { get; set; }

    public virtual DbSet<EnumType> EnumTypes { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Recipient> Recipients { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePrivilege> RolePrivileges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=MusiBuyDB_Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Mobile).HasMaxLength(15);
            entity.Property(e => e.OldPassword).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.AdminUsers)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AdminUsers_Roles");
        });

        modelBuilder.Entity<CommonSetting>(entity =>
        {
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.IsSsl).HasColumnName("IsSSL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SiteUrl)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SiteURL");
            entity.Property(e => e.Smtpserver)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SMTPServer");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.Property(e => e.Content1).HasColumnName("Content");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Page).WithMany(p => p.Contents)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Contents_Enums");
        });

        modelBuilder.Entity<Creatore>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(150)
                .HasColumnName("profilePicture");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.ProfilePicture).HasMaxLength(150);
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.Property(e => e.Subject).HasMaxLength(100);
            entity.Property(e => e.TemplateName).HasMaxLength(50);
        });

        modelBuilder.Entity<Enum>(entity =>
        {
            entity.Property(e => e.EnumValue).HasMaxLength(100);

            entity.HasOne(d => d.EnumType).WithMany(p => p.Enums)
                .HasForeignKey(d => d.EnumTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Enums_EnumTypes");

            entity.HasOne(d => d.Parant).WithMany(p => p.InverseParant)
                .HasForeignKey(d => d.ParantId)
                .HasConstraintName("FK_Enums_Enums");
        });

        modelBuilder.Entity<EnumType>(entity =>
        {
            entity.Property(e => e.EnumTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.Property(e => e.ImageName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Image_Name");
            entity.Property(e => e.MenuItemController)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuItemName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MenuItemView)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_MenuItems_MenuItems");
        });

        modelBuilder.Entity<Recipient>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);

            entity.HasOne(d => d.Template).WithMany(p => p.Recipients)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recipients_EmailTemplates");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RolePrivilege>(entity =>
        {
            entity.HasOne(d => d.MenuItem).WithMany(p => p.RolePrivileges)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePrivileges_MenuItems");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePrivileges)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolePrivileges_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
