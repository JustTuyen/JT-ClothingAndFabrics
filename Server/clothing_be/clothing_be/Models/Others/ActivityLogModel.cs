using clothing_be.Models.customers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.Others
{
    [Index(nameof(UserId))]
    [Index(nameof(TargetType), nameof(TargetId))]
    [Index(nameof(CreatedAt))]
    [Table("ActivityLog")]
    public class ActivityLogModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TargetType { get; set; } = string.Empty;


        [MaxLength(45)]
        public string IpAddress { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Action { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID là bắt buộc")]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int TargetId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
