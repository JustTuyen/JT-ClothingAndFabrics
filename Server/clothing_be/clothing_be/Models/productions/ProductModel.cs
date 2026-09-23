using clothing_be.Models.Others;
using clothing_be.Models.productions.Activity;
using clothing_be.Models.productions.Tagging;
using clothing_be.Models.productions.Varied;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions
{
    [Table("Products")]
    [Index(nameof(SubCategoryId), nameof(Name), IsUnique = true)]
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên sản phẩm không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [MaxLength(1500, ErrorMessage = "Độ dài mô tả không được vượt quá 1500 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }
        public string? Slug { get; set; }
        public int ViewCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //
        public int? DiscountId { get; set; }
        [ForeignKey(nameof(DiscountId))]
        [JsonIgnore]
        public virtual DiscountModel? Discount { get; set; }

        [Required]
        public int SubCategoryId { get; set; }
        [ForeignKey(nameof(SubCategoryId))]
        [JsonIgnore]
        public virtual SubCategoryModel? SubCategory { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public virtual ICollection<ImageGalleryModel> ImageGalleries { get; set; } = new List<ImageGalleryModel>();
        public virtual ICollection<ProductTagModel> ProductTags { get; set; } = new List<ProductTagModel>();
        public virtual ICollection<CommentModel> Comments {  get; set; } = new List<CommentModel>();
        public virtual ICollection<FavoritedModel> Favorites { get; set; } = new List<FavoritedModel>();
        public virtual ICollection<VariationModel> Variations { get; set; } = new List<VariationModel>();
    }
}
