using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.Others
{
    [Table("Banners")]
    [Index(nameof(ImageId), nameof(Name), IsUnique = true)]
    public class BannerModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên banner không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên banner không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;


        [MaxLength(500, ErrorMessage = "Đường dẫn không được vượt quá 500 ký tự")]
        public string Description { get; set; } = string.Empty;

        public string TargetUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //
        [Required]
        public int ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }

        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }


    }
}
