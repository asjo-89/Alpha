using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AlphaDbContext(DbContextOptions<AlphaDbContext> options) : DbContext(options)
{
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
    public DbSet<PictureEntity> Pictures { get; set; }
    public DbSet<ProjectEmployeeEntity> ProjectEmployees { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<ProjectNoteEntity> ProjectNotes { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<StatusEntity> Statuses { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region AddressEntity
        modelBuilder.Entity<AddressEntity>()
            .HasMany(a => a.Employees)
            .WithOne(e => e.Address)
            .HasForeignKey(e => e.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion 

        #region ClientEntity    
        modelBuilder.Entity<ClientEntity>()
            .HasMany(c => c.Projects)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId) 
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region RoleEntity
        modelBuilder.Entity<RoleEntity>()
            .HasMany(r => r.Employees)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region StatusEntity
        modelBuilder.Entity<StatusEntity>()
            .HasMany(s => s.Projects)
            .WithOne(p => p.Status)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region PictureEntity
        modelBuilder.Entity<PictureEntity>()
            .HasMany(pic => pic.Projects)
            .WithOne(p => p.Picture)
            .HasForeignKey(p => p.PictureId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PictureEntity>()
            .HasMany(pic => pic.Employees)
            .WithOne(e => e.Picture)
            .HasForeignKey(e => e.PictureId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region ProjectNoteEntity
        modelBuilder.Entity<ProjectNoteEntity>()
            .HasOne(pn => pn.Project)
            .WithMany(p => p.Notes)
            .HasForeignKey(pn => pn.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectNoteEntity>()
            .HasOne(pn => pn.Employee)
            .WithMany(e => e.Notes)
            .HasForeignKey(pn => pn.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region ProjectEntity
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.Notes)
            .WithOne(n => n.Project)
            .HasForeignKey(n => n.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.ProjectEmployees)
            .WithOne(pe => pe.Project)
            .HasForeignKey(pe => pe.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Status)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(p => p.Picture)
            .WithMany(pic => pic.Projects)
            .HasForeignKey(p => p.PictureId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region EmployeeEntity
        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.ProjectEmployees)
            .WithOne(pe => pe.Employee)
            .HasForeignKey(pe => pe.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeEntity>()
            .HasMany(e => e.Notes)
            .WithOne(n => n.Employee)
            .HasForeignKey(n => n.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Address)
            .WithMany(a => a.Employees)
            .HasForeignKey(e => e.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Role)
            .WithMany(r => r.Employees)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<EmployeeEntity>()
            .HasOne(e => e.Picture)
            .WithMany(pic =>  pic.Employees)
            .HasForeignKey(e => e.PictureId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        #region ProjectEmployeeEntity
        modelBuilder.Entity<ProjectEmployeeEntity>()
            .HasKey(pe => new { pe.ProjectId, pe.EmployeeId });

        modelBuilder.Entity<ProjectEmployeeEntity>()
            .HasOne(pe => pe.Project)
            .WithMany(p => p.ProjectEmployees)
            .HasForeignKey(pe => pe.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEmployeeEntity>()
            .HasOne(pe => pe.Employee)
            .WithMany(e => e.ProjectEmployees)
            .HasForeignKey(pe => pe.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        base.OnModelCreating(modelBuilder);
    }
}
