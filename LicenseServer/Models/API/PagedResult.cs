namespace LicenseServer.Models.API
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
