using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions
{
    [Table("Discounts")]
    public class DiscountModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(225, ErrorMessage = "Nội dung bình luận không được vượt quá 225 ký tự")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(3000, ErrorMessage = "Nội dung bình luận không được vượt quá 3000 ký tự")]
        public string Description { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Codes { get; set; }

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Percentage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime StartAt {  get; set; }
       
        [Required]
        public TimeSpan Duration { get; set; }

        public DateTime ExpireAt => StartAt.Add(Duration);

        //
        public int? ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();

    }
}
