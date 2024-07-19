using LicenseServer.Models.Database;
using Microsoft.EntityFrameworkCore;
using License = LicenseServer.Models.Database.License;

namespace LicenseServer.Database
{
    public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
		: base(options) => Database.EnsureCreated();

		public DbSet<Tarif> Tarifs { get; set; }
		public DbSet<License> Licenses { get; set; }
		public DbSet<Organization> Organizations { get; set; }
	}
}