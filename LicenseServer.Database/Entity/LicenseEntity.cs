using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Database.Entity
{
    public class LicenseEntity : BaseEntity
    {
        public virtual OrganizationEntity Organization { get; set; }
        public virtual TarifEntity Tarif { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
