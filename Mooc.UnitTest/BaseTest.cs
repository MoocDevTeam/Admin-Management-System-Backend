using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mooc.UnitTest;

public class BaseTest
{
    protected MoocWebApplicationFactory Factory { get; private set; }
    protected HttpClient Client { get; private set; }
    public BaseTest()
    {
        Factory = new MoocWebApplicationFactory();
        Client = this.Factory.CreateClient();
    }

    [OneTimeTearDown]
    protected void Clean()
    {
        if (Client != null)
        {
            Client.Dispose();
            Client = null;
        }

        if (Factory != null)
        {
            Factory.Dispose();
            Factory = null;
        }
    }

    private JsonSerializerOptions serializeOptions = new JsonSerializerOptions
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    public T? Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, serializeOptions);
    }
}
