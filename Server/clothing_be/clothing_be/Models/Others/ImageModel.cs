using clothing_be.Models.customers.Address;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Varied;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.Others
{
    [Table("Images")]
    public class ImageModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Chiều dài hình ảnh là bắt buộc")]
        public int Height { get; set; }

        [Required(ErrorMessage = "Độ rộng hình ảnh là bắt buộc")]
        public int Width { get; set; }

        [Required(ErrorMessage = "Loại hình ảnh là bắt buộc")]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ public hình ảnh là bắt buộc")]
        [MaxLength(255)]
        public string PublicId { get; set; } = string.Empty;

        [Required(ErrorMessage = "URL hình ảnh là bắt buộc")]
        [MaxLength(2048)]
        public string URL { get; set; } = string.Empty;

        public bool IsThumbnail { get; set; } = false;

        public string? AltText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //
        public virtual ICollection<ImageGalleryModel> ImageGalleries { get; set; } = new List<ImageGalleryModel>();
        //public virtual ICollection<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        //public virtual ICollection<VariationModel> Variations { get; set; } = new List<VariationModel>();
        //public virtual ICollection<BannerModel> Banners { get; set; } = new List<BannerModel>();
        //public virtual ICollection<DiscountModel> Discounts { get; set; } = new List<DiscountModel>();
    }
}
