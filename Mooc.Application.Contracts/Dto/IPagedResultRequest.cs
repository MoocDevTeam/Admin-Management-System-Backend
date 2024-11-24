namespace Mooc.Application.Contracts.Dto;

public interface IPagedResultRequest
{
    /// <summary>
    /// page index
    /// </summary>
    int PageIndex { get; set; }

    /// <summary>
    /// page size
    /// </summary>
    int PageSize { get; set; }
}
