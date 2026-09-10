using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_be.Models.productions.Varied
{
    [Table("Attributes")]
    [Index(nameof(Name), nameof(Type), IsUnique = true)]
    [Index(nameof(Type))]
    public class AttributeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên thuộc tính là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Tên thuộc tính không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại thuộc tính là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Loại thuộc tính không được vượt quá 255 ký tự")]
        public string Type { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt {  get; set; } = DateTime.UtcNow;
        
        //
        public virtual ICollection<AttributeValuesModel> AttributeValues {  get; set; } = new List<AttributeValuesModel>();
    }
}
