
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models
{
    public class ExamAnswers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ExamId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public string Answer { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}
