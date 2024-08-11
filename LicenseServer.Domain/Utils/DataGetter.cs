using LicenseServer.Database;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Utils
{
    public static class DataGetter
    {
        public static async Task<OrganizationAPI.OrganizationResponse> APIOrganizationById(int organizationId)
        {
            using var context = ApplicationContext.New;
            var organization = await context.Organizations.FirstOrDefaultAsync(o => o.Id == organizationId);

            if (organization == null)
                return null;

            var currentOrganization = new OrganizationAPI.OrganizationResponse()
            {
                Id = organization.Id,
                Name = organization.Name,
                Inn = organization.Inn,
                Kpp = organization.Kpp,
                Email = organization.Email,
                Phone = organization.Phone,
            };

            return currentOrganization;
        }

        public static async Task<List<OrganizationEntity>> PagedOrganizationEntities(int currentPage, int pageSize)
        {
            using var context = ApplicationContext.New;
            var organizations = await OrganizationsEntities();

            var pagedOrganizations = organizations
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!organizations.Any())
                return new ();
            return organizations;
        }

        public static async Task<List<OrganizationEntity>> OrganizationsEntities()
        {
            using var context = ApplicationContext.New;
            var organizations = await context.Organizations.ToListAsync();

            if (!organizations.Any())
                return new();
            return organizations;
        }

        public static async Task<OrganizationEntity> OrganizationAPIToOrganizationEntity(OrganizationAPI.OrganizationRequest organization)
        {
            OrganizationEntity currentOrganization = new()
            {
                Name = organization.Name,
                Inn = organization.Inn,
                Kpp = organization.Inn.Length == 12 ? "" : organization.Kpp,
                Email = organization.Email,
                Phone = organization.Phone
            };

            return currentOrganization;
        }

        public static async Task<List<LicenseEntity>> LicenseEntities()
        {
            using var context = ApplicationContext.New;
            var licenses = await context.Licenses
                .Include(l => l.Organization)
                .Include(l => l.Tarif)
                .ToListAsync();

            if (!licenses.Any())
                return new();
            return licenses;
        }

        public static async Task<IEnumerable<OrganizationsLiceses>> OrganizationsLiceses(int currentPage, int pageSize)
        {
            var organizations = await PagedOrganizationEntities(currentPage, pageSize);

            if (!Validator.isValidObject(organizations))
                return null;

            var licenses = await LicenseEntities();

            var data = organizations.Select(organization => new OrganizationsLiceses
            {
                Organization = organization,
                Licenses = licenses.Where(l => l.Organization != null && l.Organization.Id == organization.Id && l.Tarif != null)
                    .Select(l => new LicenseAPI.LicenseResponse
                    {
                        Id = l.Id,
                        OrganizationId = l.Organization.Id,
                        TarifId = l.Tarif.Id,
                        DateCreated = l.DateCreated,
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                    }).ToList()
            });

            return data;
        }

        public static async Task<PagedResult<OrganizationsLiceses>> PagedOrganizationsLiceses(int currentPage, int pageSize)
        {
            var organizations = await OrganizationsEntities();
            var page = new PagedResult<OrganizationsLiceses>
            {
                Items = await OrganizationsLiceses(currentPage, pageSize),
                TotalPages = (int)Math.Ceiling(organizations.Count() / (double)pageSize),
                CurrentPage = currentPage
            };
            return page;
        }
    }
}
