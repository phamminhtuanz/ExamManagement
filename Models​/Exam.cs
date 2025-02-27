using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Exam
{
    [Display(Name = "Mã Kỳ Thi")]
    public int ExamId { get; set; }

    [Display(Name = "Mã Môn Học")]
    public int? SubjectId { get; set; }

    [Display(Name = "Tiêu Đề")]
    public string Title { get; set; } = null!;

    [Display(Name = "Thời Gian (phút)")]
    public int Duration { get; set; }

    [Display(Name = "Ngày Tạo")]
    public DateTime? CreatedAt { get; set; }

    [Display(Name = "Người Tạo")]
    public int? CreatedBy { get; set; }

    [Display(Name = "Thông Tin Người Tạo")]
    public virtual Admin? CreatedByNavigation { get; set; }

    [Display(Name = "Danh Sách Kết Quả Thi")]
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    [Display(Name = "Môn Học")]
    public virtual Subject? Subject { get; set; }

    [Display(Name = "Danh Sách Câu Hỏi")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
