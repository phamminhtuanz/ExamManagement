public class ExamSubmissionModel
{
    public decimal? Score { get; set; }
    public List<StudentAnswerModel> Answers { get; set; } = new();
}

public class StudentAnswerModel
{
    public int QuestionId { get; set; }
    public string Answer { get; set; }
}
