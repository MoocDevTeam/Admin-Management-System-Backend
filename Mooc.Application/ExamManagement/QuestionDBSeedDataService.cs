using Mooc.Application.Contracts;
using Mooc.Core.MoocAttribute;
using Mooc.Core.Utils;
using StackExchange.Redis;

namespace Mooc.Application.ExamManagement
{

    [DBSeedDataOrder(3)]
    public class QuestionDBSeedDataService : IDBSeedDataService, ITransientDependency
    {
        private readonly MoocDBContext _dbContext;

        public QuestionDBSeedDataService(MoocDBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        private List<QuestionType> questionTypes = new List<QuestionType>()
        {
            new QuestionType(){Id=1, QuestionTypeName="Choice", CreatedAt=DateTime.Now, },
            new QuestionType(){Id=2, QuestionTypeName="Judgement", CreatedAt=DateTime.Now, },
            new QuestionType(){Id=3, QuestionTypeName="Short Answer", CreatedAt=DateTime.Now, },
            new QuestionType(){Id=4, QuestionTypeName="Multiple Choice", CreatedAt=DateTime.Now, },
        };

        private List<ChoiceQuestion> choiceQuestions = new List<ChoiceQuestion>()
        {
            new ChoiceQuestion(){Id=1, CourseId=1, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=1, CorrectAnswer="A"},
        };

        private List<JudgementQuestion> judgementQuestions = new List<JudgementQuestion>()
        {
            new JudgementQuestion(){Id=2, CourseId=1, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=2, CorrectAnswer=true},
        };

        private List<ShortAnsQuestion> shortAnsQuestions = new List<ShortAnsQuestion>()
        {
            new ShortAnsQuestion(){Id=3, CourseId=2, CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, QuestionBody="test", QuestionTitle="test", Marks=5, QuestionTypeId=3, ReferenceAnswer="test"},
        };

        private List<MultipleChoiceQuestion> multipleChoiceQuestions = new List<MultipleChoiceQuestion>()
        {
            new MultipleChoiceQuestion()
            {
                Id=4, 
                CourseId=1, 
                CreatedByUserId=1, 
                UpdatedByUserId=1, 
                CreatedAt=DateTime.Now, 
                UpdatedAt=DateTime.Now, 
                QuestionBody="Which of the following are object-oriented programming languages?", 
                QuestionTitle="OOP Languages", 
                Marks=5, 
                QuestionTypeId=4,
                CorrectAnswers="A,B,D"
            },
        };

        private List<Option> options = new List<Option>()
        {
            new Option(){Id=1, ChoiceQuestionId=1, OptionOrder=1, OptionValue="first option", CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, ErrorExplanation="testing"},
            new Option(){Id=2, ChoiceQuestionId=1, OptionOrder=2, OptionValue="second option", CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, ErrorExplanation="testing"},
            new Option(){Id=3, ChoiceQuestionId=1, OptionOrder=3, OptionValue="third option", CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, ErrorExplanation="testing"},
            new Option(){Id=4, ChoiceQuestionId=1, OptionOrder=4, OptionValue="fourth option", CreatedByUserId=1, UpdatedByUserId=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, ErrorExplanation="testing"},
            new Option()
            {
                Id=5, 
                MultipleChoiceQuestionId=4, 
                OptionOrder=1, 
                OptionValue="Java", 
                CreatedByUserId=1, 
                UpdatedByUserId=1, 
                CreatedAt=DateTime.Now, 
                UpdatedAt=DateTime.Now, 
                ErrorExplanation=""
            },
            new Option()
            {
                Id=6, 
                MultipleChoiceQuestionId=4, 
                OptionOrder=2, 
                OptionValue="Python", 
                CreatedByUserId=1, 
                UpdatedByUserId=1, 
                CreatedAt=DateTime.Now, 
                UpdatedAt=DateTime.Now, 
                ErrorExplanation=""
            },
            new Option()
            {
                Id=7, 
                MultipleChoiceQuestionId=4, 
                OptionOrder=3, 
                OptionValue="SQL", 
                CreatedByUserId=1, 
                UpdatedByUserId=1, 
                CreatedAt=DateTime.Now, 
                UpdatedAt=DateTime.Now, 
                ErrorExplanation="SQL is not an object-oriented programming language"
            },
            new Option()
            {
                Id=8, 
                MultipleChoiceQuestionId=4, 
                OptionOrder=4, 
                OptionValue="C++", 
                CreatedByUserId=1, 
                UpdatedByUserId=1, 
                CreatedAt=DateTime.Now, 
                UpdatedAt=DateTime.Now, 
                ErrorExplanation=""
            },
        };

        public async Task<bool> InitAsync()
        {
            // 1. insert question types
            if (!this._dbContext.QuestionType.Any())
            {
                await this._dbContext.QuestionType.AddRangeAsync(questionTypes);
                await this._dbContext.SaveChangesAsync();
            }

            // 2. insert all questions
            if (!this._dbContext.MultipleChoiceQuestion.Any())
            {
                await this._dbContext.MultipleChoiceQuestion.AddRangeAsync(multipleChoiceQuestions);
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

            // 3. insert options
            if (!this._dbContext.Option.Any())
            {
                await this._dbContext.Option.AddRangeAsync(options);
                await this._dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}