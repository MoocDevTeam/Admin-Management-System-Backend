using Mooc.Application.Contracts;
using Mooc.Core.Utils;
using Mooc.Model.Entity.ExamManagement;
using StackExchange.Redis;

namespace Mooc.Application.ExamManagement
{
    public class ExamDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public ExamDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<Exam> exam = new List<Exam>()
        {
            new Exam(){Id=1, ExamTitle="Exam1", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Manual, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=2, ExamTitle="Exam2", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=3, ExamTitle="Exam3", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
        };

        private List<ExamQuestion> examQuestions = new List<ExamQuestion>()
        {
            new ExamQuestion(){Id=1, ExamId=1, QuestionId=1, QuestionType="Choice", Marks=5, QuestionTypeId=1, QuestionOrder=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=2, ExamId=1, QuestionId=2, QuestionType="Judgement", Marks=5, QuestionTypeId=2, QuestionOrder=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=3, ExamId=1, QuestionId=3, QuestionType="Short Answer", Marks=5, QuestionTypeId=3, QuestionOrder=3, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
        };

        private List<ExamPublish> examPublish = new List<ExamPublish>()
        {
            new ExamPublish(){Id=1, ExamId=1,PublishedAt=DateTime.Now, CloseAt=DateTime.Now, },
            new ExamPublish(){Id=2, ExamId=2,PublishedAt=DateTime.Now, CloseAt=DateTime.Now, },
        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.Exam.Any())
            {
                await this._dbContext.Exam.AddRangeAsync(exam);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.ExamQuestion.Any())
            {
                await this._dbContext.ExamQuestion.AddRangeAsync(examQuestions);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.ExamPublish.Any())
            {
                await this._dbContext.ExamPublish.AddRangeAsync(examPublish);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}