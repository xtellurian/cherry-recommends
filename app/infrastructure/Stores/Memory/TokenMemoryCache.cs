using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SignalBox.Core;
using SignalBox.Core.OAuth;

namespace SignalBox.Infrastructure.Caches
{
    public class TokenMemoryCache : IM2MTokenCache
    {
        private readonly IMemoryCache cache;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<TokenMemoryCache> logger;

        public TokenMemoryCache(IDateTimeProvider dateTimeProvider, ILogger<TokenMemoryCache> logger)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
            this.cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 256 // store a maximum of 256 tokens
            });
        }


        public async Task<TokenResponse> Get(string key, System.Func<Task<TokenResponse>> resolver)
        {
            if (!cache.TryGetValue(key, out TokenResponse cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = await resolver();
                logger.LogInformation($"Resolved token, expires in {cacheEntry.ExpiresIn} seconds");
                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // make the entry aware of the size
                    .SetSize(1)
                    // Keep in cache until. Minus 1 minute as fudge factor.
                    .SetAbsoluteExpiration(dateTimeProvider.Now.AddSeconds(cacheEntry.ExpiresIn).AddMinutes(-1));

                // Save data in cache.
                cache.Set(key, cacheEntry, cacheEntryOptions);
            }
            else
            {
                logger.LogInformation("Grabbed a token from the cache");
            }

            return cacheEntry;
        }

        public Task Set(string key, TokenResponse value)
        {
            cache.Set(key, value);
            return Task.CompletedTask;
        }
    }
}