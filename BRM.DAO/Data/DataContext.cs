using System.Threading;
using System.Threading.Tasks;
using BRM.DAO.Entities;
using BRM.DAO.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BRM.DAO.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.ApplyAuditInformation();

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
