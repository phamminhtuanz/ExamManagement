using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Question
{
    [Display(Name = "Mã Câu Hỏi")]
    public int QuestionId { get; set; }

    [Display(Name = "Mã Môn Học")]
    public int? SubjectId { get; set; }

    [Display(Name = "Nội Dung Câu Hỏi")]
    public string Content { get; set; } = null!;

    [Display(Name = "Mức Độ")]
    public string Level { get; set; } = null!;

    [Display(Name = "Người Tạo")]
    public int? CreatedBy { get; set; }

    [Display(Name = "Danh Sách Đáp Án")]
    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    [Display(Name = "Thông Tin Người Tạo")]
    public virtual Admin? CreatedByNavigation { get; set; }

    [Display(Name = "Môn Học")]
    public virtual Subject? Subject { get; set; }

    [Display(Name = "Danh Sách Kỳ Thi")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
}
