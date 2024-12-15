using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface IMoocCarouselService : ICrudService<MoocCarouselDto, MoocCarouselDto, long, FilterPagedResultRequestDto, MoocCreateCarouselDto, MoocUpdateCarouselDto>
    {
        Task<MoocCarouselDto> GetByTitleAsync(string title);
    }
}
