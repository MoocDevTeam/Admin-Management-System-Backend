// using Microsoft.AspNetCore.Hosting;
// using Mooc.Application.Contracts.Course.Dto;
// using Mooc.Model.Entity.Course;

// namespace Mooc.Application.Course 
// {
//     public class SessionService : CrudService<Session, ReadSessionDto, ReadSessionDto, long, FilterPagedResultRequestDto, CreateSessionDto, UpdateSessionDto>,
//   ISessionService, ITransientDependency
//     {
//     private readonly IWebHostEnvironment _webHostEnvironment;
//     private readonly IMapper _mapper;
//     public SessionService(MoocDBContext dbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment) : base(dbContext, mapper)
//     {
//       this._webHostEnvironment = webHostEnvironment;
//       this._mapper = mapper;
//     }





//     // GetAllSessionsByCourseInstanceId 
//     public async Task<IEnumerable<ReadSessionDto>> GetAllSessionsByCourseInstanceId(long courseInstanceId)
//     {
//       // Step 1: 根据 CourseInstanceId 获取所有相关的 SessionId
//     //   var sessionIds = await this.McDBContext.CourseInstances
//     //       .Where(ci => ci.Id == courseInstanceId)
//     //       .Select(ci => ci.SessionId)
//     //       .ToListAsync();

//     //   // 检查是否找到对应的 SessionIds
//     //   if (sessionIds == null || !sessionIds.Any())
//     //   {
//     //     throw new EntityNotFoundException($"No sessions found for CourseInstanceId: {courseInstanceId}");
//     //   }

//     //   // Step 2: 获取所有相关的 Session 实体
//     //   var sessions = await this.GetQueryable()
//     //       .Where(s => sessionIds.Contains(s.Id))
//     //       .ToListAsync();

//     //   // 检查是否找到对应的 Session 实体
//     //   if (!sessions.Any())
//     //   {
//     //     throw new EntityNotFoundException($"No session data found for the provided CourseInstanceId: {courseInstanceId}");
//     //   }

//     //   // Step 3: 将实体转换为 DTO
//     //   var sessionDtos = this.Mapper.Map<IEnumerable<ReadSessionDto>>(sessions);

//     //   return sessionDtos;
//     // }

//   }

// }