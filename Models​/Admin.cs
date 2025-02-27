using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Admin
{
    [Display(Name = "Mã Quản Trị Viên")]
    public int AdminId { get; set; }

    [Display(Name = "Tên Đăng Nhập")]
    public string Username { get; set; } = null!;

    [Display(Name = "Mật Khẩu")]
    public string Password { get; set; } = null!;

    [Display(Name = "Họ và Tên")]
    public string? FullName { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Danh Sách Kỳ Thi")]
    public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    [Display(Name = "Danh Sách Câu Hỏi")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
