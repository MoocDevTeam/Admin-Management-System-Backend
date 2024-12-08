using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Mooc.Core.Caching;

namespace Mooc.UnitTest;

public class MoocCacheTest : BaseTest
{
    [Test]
    public async Task Test()
    {
        using (var scope = Factory.Services.CreateScope())
        {
            var moocCache = scope.ServiceProvider.GetRequiredService<IMoocCache>();
            string key = "key1";
            string val = "123dadfdsaf";
            await moocCache.SetAsync(key, val, 5);
            var cVal =await moocCache.GetAsync<string>(key);
            Assert.That(val == cVal, "");
        }
    }
}
