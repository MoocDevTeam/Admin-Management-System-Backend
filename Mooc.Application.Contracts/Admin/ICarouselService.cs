using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Admin
{
    public interface ICarouselService : ICrudService<CarouselDto, CarouselDto, long, FilterPagedResultRequestDto, CreateCarouselDto, UpdateCarouselDto>
    {
        Task<CarouselDto> GetByTitleAsync(string title);
    }
}
