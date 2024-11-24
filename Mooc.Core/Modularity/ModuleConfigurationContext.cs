using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Mooc.Core.Modularity;

public class ModuleConfigurationContext
{
    public IServiceCollection Services { get; }

    public IDictionary<string, object> Items { get; }

    /// <summary>
    /// Gets/sets arbitrary named objects those can be stored during
    /// the service registration phase and shared between modules.
    ///
    /// This is a shortcut usage of the <see cref="Items"/> dictionary.
    /// Returns null if given key is not found in the <see cref="Items"/> dictionary.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public object this[string key]
    {
        get => Items.TryGetValue(key, out var obj) ? obj : default;
        set => Items[key] = value;
    }

    public ModuleConfigurationContext([NotNull] IServiceCollection services)
    {
        Items = new Dictionary<string, object>();
    }
}
