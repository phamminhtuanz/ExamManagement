using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models
{
    public class ExamResultModel
    {
        [Required]
        public int ExamId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int Score { get; set; }

        [Required]
        public List<AnswerModel> Answers { get; set; } = new List<AnswerModel>();
    }

    public class AnswerModel
    {
        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Answer { get; set; } = string.Empty;
    }
}
