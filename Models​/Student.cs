using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models;

public partial class Student
{
    [Display(Name = "Mã Sinh Viên")]
    public int StudentId { get; set; }

    [Display(Name = "Tên Đăng Nhập")]
    public string Username { get; set; } = null!;

    [Display(Name = "Mật Khẩu")]
    public string Password { get; set; } = null!;

    [Display(Name = "Trường Học")]
    public string School { get; set; } = null!;

    [Display(Name = "Họ và Tên")]
    public string? FullName { get; set; }

    [Display(Name = "Email")]
    public string? Email { get; set; }

    [Display(Name = "Danh Sách Kết Quả Thi")]
    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}
