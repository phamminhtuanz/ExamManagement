using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class ExamResult
{
    [Display(Name = "Mã Kết Quả")]
    public int ResultId { get; set; }

    [Display(Name = "Mã Sinh Viên")]
    public int? StudentId { get; set; }

    [Display(Name = "Mã Kỳ Thi")]
    public int? ExamId { get; set; }

    [Display(Name = "Điểm Số")]
    public decimal? Score { get; set; }
    public string? MaCauTL {  get; set; }
    [Display(Name = "Ngày Nộp Bài")]
    public DateTime? SubmittedAt { get; set; }

    [Display(Name = "Kỳ Thi")]
    public virtual Exam? Exam { get; set; }

    [Display(Name = "Sinh Viên")]
    public virtual Student? Student { get; set; }
}
