using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FF_Fastfood.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]
        public string Address { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpiration { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
    }


    public class VerifyResetCodeViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã xác nhận.")]
        public string ResetCode { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}
