using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SD106_Onewhero_Assessment_2.View;
using System.ComponentModel.DataAnnotations.Schema;


namespace SD106_Onewhero_Assessment_2.Model
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        [MaxLength(150)]
        public string? Email { get; set; }

        [Required]
        [Column("password_hash")]
        [MaxLength(255)]
        public string? PasswordHash { get; set; }

        [Required]
        [Column("phone")]
        [MaxLength(50)]
        public string? Phone { get; set; }

        [Required]
        [Column("role")]
        public string? Role { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Column ("Interest_name")]
        public string Interests { get; set; } = "";

        // One-to-many relationship with Booking
        public List<Booking>? Bookings { get; set; }
    }
}
