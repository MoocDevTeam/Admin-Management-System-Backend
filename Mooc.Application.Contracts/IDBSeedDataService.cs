namespace Mooc.Application.Contracts;

public interface IDBSeedDataService
{
    Task<bool> InitAsync();
}
