namespace Mooc.Application.Contracts.Dto;

public class PagedResultRequestDto : IPagedResultRequest, ISortedResultRequest
{
    public int PageIndex { get ; set ; }
    public int PageSize { get ; set ; }
    public string? Sorting { get; set ; }
}
