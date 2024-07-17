namespace LicenseServer.Models.Database
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }
}
