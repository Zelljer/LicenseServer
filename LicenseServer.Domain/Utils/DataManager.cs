using LicenseServer.Database;

namespace LicenseServer.Domain.Utils
{
    public class DataManager
    {
        public static async Task AddEntityAsync<T>(T entity, ApplicationContext context = null) where T : class
        {
            if (context == null)
            {
                using var newContext = ApplicationContext.New;
                newContext.Set<T>().Add(entity);
                await newContext.SaveChangesAsync();
            }
            else
            {
                context.Set<T>().Add(entity);
                await context.SaveChangesAsync();
            }
        }

        public static async Task RemoveEntityAsync<T>(T entity) where T : class
        {
            using var context = ApplicationContext.New;
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}
