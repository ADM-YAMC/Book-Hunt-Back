﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainLayer.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public Role? Role { get; set; }
    }
}
