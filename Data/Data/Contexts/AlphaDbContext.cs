using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AlphaDbContext(DbContextOptions<AlphaDbContext> options) : IdentityDbContext<MemberUserEntity, IdentityRole<Guid>, Guid>(options)
{
    public virtual DbSet<AddressEntity> Addresses { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<PictureEntity> Pictures { get; set; }
    public virtual DbSet<ProjectEntity> Projects { get; set; }
    public virtual DbSet<ProjectMemberEntity> ProjectMembers { get; set; }
    public virtual DbSet<ProjectNoteEntity> ProjectNotes { get; set; }
    //public virtual DbSet<StatusEntity> Statuses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ProjectMemberEntity>()
            .HasKey(pm => new { pm.MemberId, pm.ProjectId });

        #region MemberUserEntity

        builder.Entity<MemberUserEntity>()
            .HasOne(mu => mu.Picture)
            .WithMany(p => p.Members)
            .HasForeignKey(mu => mu.PictureId)
            .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region AddressEntity

        builder.Entity<AddressEntity>()
            .HasOne(a => a.Member)
            .WithOne(mu => mu.Address)
            .HasForeignKey<AddressEntity>(a => a.MemberUserId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        #region ProjectEntity

        builder.Entity<ProjectEntity>()
            .HasOne(p => p.Picture)
            .WithMany(pic => pic.ProjectEntities)
            .HasForeignKey(p => p.PictureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProjectEntity>()
            .HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        //builder.Entity<ProjectEntity>()
        //    .HasOne(p => p.Status)
        //    .WithMany(s => s.Projects)
        //    .HasForeignKey(p => p.StatusId)
        //    .OnDelete(DeleteBehavior.Restrict);

        #endregion

        #region ProjectNoteEntity

        builder.Entity<ProjectNoteEntity>()
            .HasOne(pn => pn.Member)
            .WithMany(m => m.ProjectNotes)
            .HasForeignKey(pn => pn.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProjectNoteEntity>()
            .HasOne(pn => pn.Project)
            .WithMany(p => p.ProjectNotes)
            .HasForeignKey(pn => pn.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion

        base.OnModelCreating(builder);
    }
}
