using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Mooc.Model.DBContext;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.UnitTest
{
    public abstract class BaseTest
    {
        protected readonly Mock<IWebHostEnvironment> MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
        protected readonly IMapper Mapper;
        protected DbContextOptions<MoocDBContext> DBOption = new DbContextOptionsBuilder<MoocDBContext>()
         .UseInMemoryDatabase("MoocDBContext_DB")
         .Options;
        protected abstract IMapper CreateMap();

        protected BaseTest()
        {
            Mapper = CreateMap();
        }
    }
}
