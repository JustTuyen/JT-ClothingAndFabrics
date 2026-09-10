using clothing_be.Models.customers;
using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions.Activity
{
    [Table("Comments")]
    [Index(nameof(UserId), nameof(ProductId), IsUnique = true)]
    public class CommentModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(225, ErrorMessage = "Nội dung bình luận không được vượt quá 225 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000, ErrorMessage = "Nội dung bình luận không được vượt quá 2000 ký tự")]
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Range(1, 5)]
        public int? Rating { get; set; }
        //
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public virtual ProductModel? Product { get; set; }

        [Required]
        public int StatusID { get; set; }
        [ForeignKey(nameof(StatusID))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
    }
}
