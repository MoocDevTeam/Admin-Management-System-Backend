using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using Mooc.Model.Entity.ExamManagement;
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
            new Exam(){Id=1, ExamTitle="Exam1", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Manual, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=2, ExamTitle="Exam2", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
            new Exam(){Id=3, ExamTitle="Exam3", Remark=100, ExaminationTime=120, AutoOrManual=QuestionUpload.Auto, TotalScore=100, TimePeriod=120, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now,},
        };

        private List<ExamQuestion> examQuestions = new List<ExamQuestion>()
        {
            new ExamQuestion(){Id=1, ExamId=1, ChoiceQuestionId=1, Marks=5, QuestionOrder=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=2, ExamId=1, JudgementQuestionId=2, Marks=5, QuestionOrder=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
            new ExamQuestion(){Id=3, ExamId=1, ShortAnsQuestionId=3, Marks=5, QuestionOrder=3, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, },
        };

        private List<ExamPublish> examPublish = new List<ExamPublish>()
        {
            new ExamPublish(){Id=1, ExamId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, CloseAt=DateTime.Now, },
            new ExamPublish(){Id=2, ExamId=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, CloseAt=DateTime.Now, },
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