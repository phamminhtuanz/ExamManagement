using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Answer
{
    [Display(Name = "Mã Câu Trả Lời")]
    public int AnswerId { get; set; }

    [Display(Name = "Mã Câu Hỏi")]
    public int? QuestionId { get; set; }

    [Display(Name = "Nội Dung")]
    public string Content { get; set; } = null!;

    [Display(Name = "Đáp Án Đúng")]
    public bool IsCorrect { get; set; }

    [Display(Name = "Câu Hỏi")]
    public virtual Question? Question { get; set; }
}
