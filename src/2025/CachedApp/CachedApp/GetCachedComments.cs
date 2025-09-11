using Microsoft.Extensions.Caching.Memory;

namespace CachedApp;

internal sealed class GetCachedComments(IGetComments getComments, IMemoryCache cache) : IGetComments
{
    public async Task<IReadOnlyList<Comment>> GetAsync(CancellationToken cancellationToken)
    {
        const string commentsKey = "Comments";

        var comments = await cache.GetOrCreateAsync(commentsKey, cacheEntry =>
        {
            cacheEntry.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(2);
            
            return getComments.GetAsync(cancellationToken);
        });

        return comments!;
    }
}