using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Subject
{
    [Display(Name = "Mã Môn Học")]
    public int SubjectId { get; set; }

    [Display(Name = "Tên Môn Học")]
    public string SubjectName { get; set; } = null!;

    [Display(Name = "Mô Tả")]
    public string? Description { get; set; }

    [Display(Name = "Danh Sách Kỳ Thi")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    [Display(Name = "Danh Sách Câu Hỏi")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
