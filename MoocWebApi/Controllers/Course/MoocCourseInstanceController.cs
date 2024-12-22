namespace MoocWebApi.Controllers.Course
{
    [ApiExplorerSettings(GroupName = nameof(SwaggerGroup.CourseService))]
    [Route("api/[controller]")]
    [ApiController]
    public class MoocCourseInstanceController : ControllerBase
    {
        // GET: api/<MoocCourseInstanceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MoocCourseInstanceController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MoocCourseInstanceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MoocCourseInstanceController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MoocCourseInstanceController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
