using LicenseServer.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Database
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext() {}

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LicenseServer;Trusted_Connection=True;Trust Server Certificate=True;");
		}

		public DbSet<TarifEntity> Tarifs { get; set; }
        public DbSet<LicenseEntity> Licenses { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
		public DbSet<UserEntity> Users { get; set; }

		public static ApplicationContext New => new();

    }
}