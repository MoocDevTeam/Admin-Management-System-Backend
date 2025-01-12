using Amazon.S3.Model;
using Microsoft.AspNetCore.Hosting;
using Mooc.Application.Contracts.Course.Dto.Category;

namespace Mooc.Application.Course;

public class EnrollmentService : CrudService<Enrollment, EnrollmentDto, EnrollmentDto, long, FilterPagedResultRequestDto, CreateEnrollmentDto, UpdateEnrollmentDto>,
    IEnrollmentService, ITransientDependency

{
   
    public EnrollmentService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
        
    }

    protected virtual async Task ValidateEnrollmentIdAsync(long id)
    {
        var exit = await base.GetQueryable().AnyAsync(x => x.Id == id);
        if (!exit)
        {
            throw new EntityNotFoundException($"Category with Id {id} is not found");
        }
    }

    public override async Task<EnrollmentDto> UpdateAsync(long id, UpdateEnrollmentDto input)
    {
        await ValidateEnrollmentIdAsync(input.Id);     
        return await base.UpdateAsync(input.Id, input);
    }

}
