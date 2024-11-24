namespace Mooc.Core.Modularity;

public class DependsOnAttribute : Attribute
{
    public Type[] DependedTypes { get; }

    public DependsOnAttribute(params Type[] dependedTypes)
    {
        DependedTypes = dependedTypes ?? new Type[0];
    }

    public virtual Type[] GetDependedTypes()
    {
        return DependedTypes;
    }
}
