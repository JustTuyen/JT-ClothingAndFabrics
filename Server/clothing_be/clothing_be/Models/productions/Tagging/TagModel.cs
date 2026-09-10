using clothing_be.Models.Others;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions.Tagging
{
    public class TagModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên tag là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên tag không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //forgien connects
        [Required]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
        public virtual ICollection<ProductTagModel> ProductTags { get; set; } = new List<ProductTagModel>();
    }
}
