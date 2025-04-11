﻿using System.ComponentModel.DataAnnotations;

namespace Hotel360InteractiveServer.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
