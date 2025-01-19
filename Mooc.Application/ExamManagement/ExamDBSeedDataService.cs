using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using StackExchange.Redis;

namespace Mooc.Application.ExamManagement
{

    [DBSeedDataOrder(4)]
    public class ExamDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public ExamDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<Exam> exam = new List<Exam>()
        {
            new Exam(){Id=1, CourseId=1, ExamTitle="Exam1", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Manual, TotalScore=100, TimePeriod=120, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=2, CourseId=2, ExamTitle="Exam2", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=3, CourseId=1, ExamTitle="Exam3", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
        };

        private List<ExamQuestion> examQuestions = new List<ExamQuestion>()
        {
            new ExamQuestion(){Id=1, ExamId=1, ChoiceQuestionId=1, Marks=5, QuestionOrder=1, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=2, ExamId=2, JudgementQuestionId=2, Marks=5, QuestionOrder=2, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=3, ExamId=1, ShortAnsQuestionId=3, Marks=5, QuestionOrder=3, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
        };

        private List<ExamPublish> examPublish = new List<ExamPublish>()
        {
            new ExamPublish(){Id=1, ExamId=1, CreatedAt=DateTime.Now, CreatedByUserId=1, UpdatedByUserId=1, UpdatedAt=DateTime.Now, CloseAt=DateTime.Now, },
            new ExamPublish(){Id=2, ExamId=2, CreatedAt=DateTime.Now, CreatedByUserId=1, UpdatedByUserId=1, UpdatedAt=DateTime.Now, CloseAt=DateTime.Now, },
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