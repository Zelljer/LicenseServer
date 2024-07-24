using LicenseServer.Database.Entity;
using Microsoft.EntityFrameworkCore;
using LicenseEntity = LicenseServer.Database.Entity.LicenseEntity;

namespace LicenseServer.Database
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<TarifEntity> Tarifs { get; set; }
        public DbSet<LicenseEntity> Licenses { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }

    }
}