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

        #region ProjectEntity
        modelBuilder.Entity<ProjectEntity>()
            .HasMany(p => p.Notes)
            .WithOne(n => n.Project)
            .HasForeignKey(n => n.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);
        #endregion

        base.OnModelCreating(modelBuilder);
    }
}
