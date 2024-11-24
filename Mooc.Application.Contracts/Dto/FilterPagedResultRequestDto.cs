namespace Mooc.Application.Contracts.Dto
{
    public class FilterPagedResultRequestDto : PagedResultRequestDto, IFilterResultRequest
    {
        public string? Filter { get; set; }
    }
}
