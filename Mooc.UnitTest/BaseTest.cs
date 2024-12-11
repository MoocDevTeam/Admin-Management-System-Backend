using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mooc.UnitTest;

public class BaseTest
{
    protected MoocWebApplicationFactory Factory { get; private set; }
    public BaseTest()
    {
        Factory = new MoocWebApplicationFactory();
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
