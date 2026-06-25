using Microsoft.Extensions.Caching.Distributed;

namespace Online_Store_ASP.NET_Core_MVC.Services
{
    public class CachedProductRepository
    {
            private readonly IDistributedCache _cache;
            private readonly IProductRepository _repository;


            public CachedProductRepository(
                IDistributedCache cache,
                IProductRepository repository)
            {
                _cache = cache;
                _repository = repository;
            }
        }
}
