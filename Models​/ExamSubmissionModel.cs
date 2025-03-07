public class ExamSubmissionModel
{
    public decimal? Score { get; set; }
    public List<StudentAnswerModel> Answers { get; set; } = new();
    public string MaCauTL { get; set; }
}

public class StudentAnswerModel
{
    public int QuestionId { get; set; }
    public string Answer { get; set; }
}
