using Microsoft.AspNetCore.Hosting;

namespace Mooc.Application.Course;

public class EnrollmentService : CrudService<Enrollment, EnrollmentDto, EnrollmentDto, long, FilterPagedResultRequestDto, CreateEnrollmentDto, UpdateEnrollmentDto>,
    IEnrollmentService, ITransientDependency

{
    private readonly IWebHostEnvironment _webHostEnvironment;
   
    public EnrollmentService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
    {
        this._webHostEnvironment = webHostEnvironment;
    }
}
