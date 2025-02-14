﻿using Mooc.Model.Entity;
namespace Mooc.Application.Contracts.ExamManagement;

public class ExamDto : BaseEntityDto
{
    public long CourseId { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public long? UpdatedByUserId { get; set; }

    public DateTime? UpdatedAt { get; set; }


    public string ExamTitle { get; set; }

    public int? Remark { get; set; }

    public int ExaminationTime { get; set; }

    public QuestionUpload AutoOrManual { get; set; }

    public int TotalScore { get; set; }

    public int TimePeriod { get; set; }

    public List<ExamQuestionDto>? ExamQuestions { get; set; }

    public ExamPublishDto? ExamPublish { get; set; }

}