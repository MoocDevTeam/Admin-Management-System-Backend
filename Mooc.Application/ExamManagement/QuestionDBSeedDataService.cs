using Mooc.Application.Contracts;
using Mooc.Core.Utils;
using Mooc.Model.Entity.ExamManagement;
using StackExchange.Redis;

namespace Mooc.Application.ExamManagement
{
    public class QuestionDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public QuestionDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<QuestionType> questionTypes = new List<QuestionType>()
        {
            new QuestionType(){Id=1, QuestionTypeName="Choice", },
            new QuestionType(){Id=2, QuestionTypeName="Judgement", },
            new QuestionType(){Id=3, QuestionTypeName="Short Answer", },
        };

        private List<ChoiceQuestion> choiceQuestions = new List<ChoiceQuestion>()
        {
            new ChoiceQuestion(){Id=1, CreatedByUserId=1, CreatedAt=DateTime.Now, UpdatedByUserId=1, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=1, CorrectAnswer="A"},
        };

        private List<JudgementQuestion> judgementQuestions = new List<JudgementQuestion>()
        {
            new JudgementQuestion(){Id=1, CreatedByUserId=1, CreatedAt=DateTime.Now, UpdatedByUserId=1, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=2, CorrectAnswer=true},
        };

        private List<ShortAnsQuestion> shortAnsQuestions = new List<ShortAnsQuestion>()
        {
            new ShortAnsQuestion(){Id=1, CreatedByUserId=1, CreatedAt=DateTime.Now, UpdatedByUserId=1, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=3, ReferenceAnswer="test"},
        };

        private List<Option> options = new List<Option>()
        {
            new Option(){Id=1, ChoiceQuestionId=1, OptionOrder=1, OptionValue="first option", CreatedByUserId=1, CreatedAt=DateTime.Now, UpdatedByUserId=1, UpdatedAt=DateTime.Now, ErrorExplanation="testing"},
        };

        public async Task<bool> InitAsync()
        {
            if (!this._dbContext.QuestionType.Any())
            {
                await this._dbContext.QuestionType.AddRangeAsync(questionTypes);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.ChoiceQuestion.Any())
            {
                await this._dbContext.ChoiceQuestion.AddRangeAsync(choiceQuestions);
                await this._dbContext.SaveChangesAsync();
            }
            if (!this._dbContext.JudgementQuestion.Any())
            {
                await this._dbContext.JudgementQuestion.AddRangeAsync(judgementQuestions);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.ShortAnsQuestion.Any())
            {
                await this._dbContext.ShortAnsQuestion.AddRangeAsync(shortAnsQuestions);
                await this._dbContext.SaveChangesAsync();
            }

            if (!this._dbContext.Option.Any())
            {
                await this._dbContext.Option.AddRangeAsync(options);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}