using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ComplaintBox.Models;

namespace ComplaintBox.Models;

public partial class ComplaintBoxDbContext : DbContext
{
    public ComplaintBoxDbContext()
    {
    }

    public ComplaintBoxDbContext(DbContextOptions<ComplaintBoxDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AreaInfo> AreaInfos { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserComplaint> UserComplaints { get; set; }

    public virtual DbSet<VictimInfo> VictimInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ComplaintBoxDB;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE488EBAF8F26");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AdminPass)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AdminStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AreaInfo>(entity =>
        {
            entity.HasKey(e => e.PinCode).HasName("PK__Area_Inf__70964C4EE36FBBC2");

            entity.ToTable("Area_Info");

            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AdminId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AreaName)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Admin).WithMany(p => p.AreaInfos)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Area_Info_Admin");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.ComplaintId).HasName("PK__Complain__740D898FF9FAB046");

            entity.Property(e => e.ComplaintId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.AdminAllotedId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.BuildingNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ComplaintStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ComplaintType)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.DateTimeLodged).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Images).HasColumnType("image");
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.StreetNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VictimId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.AdminAlloted).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.AdminAllotedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Complaints_Admin");

            entity.HasOne(d => d.PinCodeNavigation).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.PinCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Complaints_Area_Info");

            entity.HasOne(d => d.Victim).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.VictimId)
                .HasConstraintName("FK_Complaints_Victim_Info");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB45BB1B3");

            entity.ToTable(tb => tb.HasTrigger("trgGenerateUserId"));

            entity.HasIndex(e => e.EmailId, "UQ__Users__7ED91ACE0B29CB8A").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EmailId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<UserComplaint>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.ComplaintId }).HasName("PK__User_Com__F0C814D4D110AF4D");

            entity.ToTable("User_Complaints");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ComplaintId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Complaint).WithMany(p => p.UserComplaints)
                .HasForeignKey(d => d.ComplaintId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Complaints_Complaints");
        });

        modelBuilder.Entity<VictimInfo>(entity =>
        {
            entity.HasKey(e => e.VictimId).HasName("PK__Victim_I__1ABDABCBE2F0A544");

            entity.ToTable("Victim_Info");

            entity.Property(e => e.VictimId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.VictimGender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.VictimName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserEmail>().HasKey(e => e.EmailId);

        modelBuilder.Entity<ComplaintInfo>().HasKey(Ci => Ci.PinCode);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

public DbSet<ComplaintBox.Models.UserEmail> UserEmail { get; set; } = default!;

public DbSet<ComplaintBox.Models.ComplaintInfo> ComplaintInfo { get; set; } = default!;
}
