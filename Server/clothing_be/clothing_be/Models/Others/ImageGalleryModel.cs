using clothing_be.Models.customers.Address;
using clothing_be.Models.productions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.Others
{
    [Table("ImageGalleries")]
    [Index(nameof(ProductId), nameof(ImageId))]
    public class ImageGalleryModel
    {
        [Key]
        public int Id { get; set; }
        public int DisplayOrder { get; set; } = 0;

        [MaxLength(2000)]
        public string Note { get; set; } = string.Empty;
        [Required(ErrorMessage = "product ID là bắt buộc")]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public virtual ProductModel? Product { get; set; }

        [Required(ErrorMessage = "Image ID là bắt buộc")]
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }
    }
}
