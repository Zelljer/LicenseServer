using LicenseServer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LicenseServer.Domain.Utils
{
    public static class HttpResults
    {
        public static class StringResult
        {
            public static HTTPResult<string> Success(string data)
            {
                return new HTTPResult<string> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<string> Fails(List<string> data)
            {
                return new HTTPResult<string> { Errors = data, IsSuccsess = false };
            }

            public static HTTPResult<string> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<string> Empty()
            {
                return new HTTPResult<string> { IsSuccsess = true };
            }
        }

        public static class OrganizationResult
        {
            public static HTTPResult<OrganizationAPI.OrganizationResponse> Success(OrganizationAPI.OrganizationResponse data)
            {
                return new HTTPResult<OrganizationAPI.OrganizationResponse> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<OrganizationAPI.OrganizationResponse> Fails(List<string> data)
            {
                return new HTTPResult<OrganizationAPI.OrganizationResponse> { Errors = data , IsSuccsess = false };
            }

            public static HTTPResult<OrganizationAPI.OrganizationResponse> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<OrganizationAPI.OrganizationResponse> Empty()
            {
                return new HTTPResult<OrganizationAPI.OrganizationResponse> { IsSuccsess = true };
            }
        }

        public static class OrganizationsPagedResult
        {
            public static HTTPResult<PagedResult<OrganizationsLiceses>> Success(PagedResult<OrganizationsLiceses> data)
            {
                return new HTTPResult<PagedResult<OrganizationsLiceses>> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<PagedResult<OrganizationsLiceses>> Fails(List<string> data)
            {
                return new HTTPResult<PagedResult<OrganizationsLiceses>> { Errors = data, IsSuccsess = false };
            }

            public static HTTPResult<PagedResult<OrganizationsLiceses>> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<PagedResult<OrganizationsLiceses>> Empty()
            {
                return new HTTPResult<PagedResult<OrganizationsLiceses>> { IsSuccsess = true };
            }
        }

        public static class TarifResult
        {
            public static HTTPResult<TarifAPI.TarifResponse> Success(TarifAPI.TarifResponse data)
            {
                return new HTTPResult<TarifAPI.TarifResponse> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<TarifAPI.TarifResponse> Fails(List<string> data)
            {
                return new HTTPResult<TarifAPI.TarifResponse> { Errors = data, IsSuccsess = false };
            }

            public static HTTPResult<TarifAPI.TarifResponse> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<TarifAPI.TarifResponse> Empty()
            {
                return new HTTPResult<TarifAPI.TarifResponse> { IsSuccsess = true };
            }
        }

        public static class TarifsResult
        {
            public static HTTPResult<List<TarifAPI.TarifResponse>> Success(List<TarifAPI.TarifResponse> data)
            {
                return new HTTPResult<List<TarifAPI.TarifResponse>> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<List<TarifAPI.TarifResponse>> Fails(List<string> data)
            {
                return new HTTPResult<List<TarifAPI.TarifResponse>> { Errors = data, IsSuccsess = false };
            }

            public static HTTPResult<List<TarifAPI.TarifResponse>> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<List<TarifAPI.TarifResponse>> Empty()
            {
                return new HTTPResult<List<TarifAPI.TarifResponse>> { IsSuccsess = true };
            }
        }

        public static class LicensesResult
        {
            public static HTTPResult<List<LicenseAPI.LicenseResponse>> Success(List<LicenseAPI.LicenseResponse> data)
            {
                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Data = data, IsSuccsess = true };
            }

            public static HTTPResult<List<LicenseAPI.LicenseResponse>> Fails(List<string> data)
            {
                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { Errors = data, IsSuccsess = false };
            }

            public static HTTPResult<List<LicenseAPI.LicenseResponse>> Fail(string data)
            {
                return Fails([data]);
            }

            public static HTTPResult<List<LicenseAPI.LicenseResponse>> Empty()
            {
                return new HTTPResult<List<LicenseAPI.LicenseResponse>> { IsSuccsess = true };
            }
        }
    }
}
