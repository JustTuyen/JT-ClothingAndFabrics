using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions
{
    [Table("SubCategories")]
    [Index(nameof(CategoryId), nameof(Name), IsUnique = true)]
    [Index(nameof(Slug), IsUnique = true)]
    public class SubCategoryModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên danh mục không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1500, ErrorMessage = "Độ dài mô tả không được vượt quá 1500 ký tự")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [JsonIgnore]
        public virtual CategoryModel? Category { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
