using System.ComponentModel.DataAnnotations;

namespace ExamManagement.Models
{
    public class LoginC
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mật Khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi Nhớ Đăng Nhập")]
        public bool Remember { get; set; }
    }
}
