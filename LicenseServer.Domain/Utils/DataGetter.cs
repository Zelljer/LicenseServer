using LicenseServer.Database;
using LicenseServer.Database.Dependencies;
using LicenseServer.Database.Entity;
using LicenseServer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LicenseServer.Domain.Utils
{
    public static class DataGetter
    {

        #region ByID
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

        public static async Task<TarifAPI.TarifResponse> TarifById(int tarifId)
        {
            using var context = ApplicationContext.New;
            var tarif = await context.Tarifs.FindAsync(tarifId);

            if (tarif == null)
                return null;

            var currentTarif = new TarifAPI.TarifResponse()
            {
                Id = tarif.Id,
                Name = tarif.Name,
                Program = tarif.Program.ToString(),
                Price = tarif.Price,
                DaysCount = tarif.DaysCount
            };

            return currentTarif;
        }

        public static async Task<LicenseEntity> LicenseEntityById(int licenseId)
        {
            using var context = ApplicationContext.New;
            var currentLicense = await context.Licenses.FirstOrDefaultAsync(u => u.Id == licenseId);

            return currentLicense;
        }

        public static async Task<List<LicenseAPI.LicenseResponse>> APILicensesByOrganizationId(int organizationId)
        {
            using var context = ApplicationContext.New;
            var licenses = await context.Licenses
                .Include(l => l.Organization)
                .Include(l => l.Tarif)
                .Where(l => l.Organization.Id == organizationId && l.EndDate > DateTime.Now)
                .Select(t => new LicenseAPI.LicenseResponse
                {
                    Id = t.Id,
                    OrganizationId = t.Organization.Id,
                    TarifId = t.Tarif.Id,
                    DateCreated = t.DateCreated,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                }).ToListAsync();

            return licenses;
        }

        public static async Task<List<LicenseAPI.LicenseResponse>> APILicensesByOrganizationIdWithProgramId(int organizationId, ProgramType programId)
        {
            using var context = ApplicationContext.New;
            var licenses = await context.Licenses
                     .Include(l => l.Organization)
                     .Include(l => l.Tarif)
                     .Where(l => l.Organization.Id == organizationId && l.Tarif.Program == programId && l.EndDate > DateTime.Now)
                     .Select(t => new LicenseAPI.LicenseResponse
                     {
                         Id = t.Id,
                         OrganizationId = t.Organization.Id,
                         TarifId = t.Tarif.Id,
                         DateCreated = t.DateCreated,
                         StartDate = t.StartDate,
                         EndDate = t.EndDate,
                     }).ToListAsync();

            return licenses;
        }
        #endregion

        #region APIToEntity
        public static OrganizationEntity OrganizationAPIToOrganizationEntity(OrganizationAPI.OrganizationRequest organization)
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

        public static UserEntity UserAPIToUserEntity(UserAPI.UserRegistrationRequest user)
        {
            var currentUser = new UserEntity
            {
                Name = user.Name,
                Surname = user.Surname,
                Patronymic = user.Patronymic,
                Login = user.Login,
                Password = Hasher.HashPassword(user.Password),
                Role = user.Role,
            };

            return currentUser;
        }

        public static async Task<LicenseEntity> APILicenseToLicenseEntity(LicenseAPI.LicenseRequest license, ApplicationContext context)
        {
            var neededOrganization = await context.Organizations.FindAsync(license.OrganizationId);
            var neededTarif = await context.Tarifs.FindAsync(license.TarifId);

            var currentLicense = new LicenseEntity
            {
                Organization = neededOrganization,
                Tarif = neededTarif,
                DateCreated = DateTime.Now,
                StartDate = DateTime.Parse(license.DateStart),
                EndDate = DateTime.Parse(license.DateStart).AddDays(neededTarif.DaysCount),
            };

            return currentLicense;
        }

        public static TarifEntity TarifAPIToTarifEntity(TarifAPI.TarifRequest tarif)
        {
            TarifEntity currentTarif = new TarifEntity()
            {
                Name = tarif.Name,
                Program = tarif.Program,
                Price = tarif.Price,
                DaysCount = tarif.DaysCount
            };

            return currentTarif;
        }

        public static async Task<UserEntity> UserEntityByLogin(UserAPI.UserAuthentificationRequest user)
        {
            using var context = ApplicationContext.New;
            var currentUser = await context.Users.FirstOrDefaultAsync(u => u.Login == user.Login);

            return currentUser;
        }
        #endregion

        #region Paged

        public static async Task<List<OrganizationEntity>> OrganizationEntitiesByPageSettings(int currentPage, int pageSize)
        {
            using var context = ApplicationContext.New;
            var organizations = await OrganizationsEntities();

            var pagedOrganizations = organizations
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (!pagedOrganizations.Any())
                return new();
            return pagedOrganizations;
        }

        public static async Task<IEnumerable<OrganizationsLiceses>> OrganizationsLiceses(int currentPage, int pageSize)
        {
            var organizations = await OrganizationEntitiesByPageSettings(currentPage, pageSize);

            if (!Validator.isValidObject(organizations))
                return Enumerable.Empty<OrganizationsLiceses>();

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

        #endregion

        #region Lists

        public static async Task<List<OrganizationEntity>> OrganizationsEntities()
        {
            using var context = ApplicationContext.New;
            var organizations = await context.Organizations.ToListAsync();

            if (!organizations.Any())
                return new();
            return organizations;
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

        public static async Task<List<TarifAPI.TarifResponse>> ListTarifAPI()
        {
            using var context = ApplicationContext.New;
            var apiTarifs = await context.Tarifs.Select(t => new TarifAPI.TarifResponse
            {
                Id = t.Id,
                Name = t.Name,
                Program = t.Program.ToString(),
                Price = t.Price,
                DaysCount = t.DaysCount,

            }).ToListAsync();

            return apiTarifs;
        }
        #endregion

    }
}
