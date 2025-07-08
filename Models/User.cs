﻿using System.ComponentModel.DataAnnotations;

namespace ClothingStoreApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        public string? Role { get; set; }
    }
}

