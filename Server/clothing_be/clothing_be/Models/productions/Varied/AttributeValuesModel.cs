using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace clothing_be.Models.productions.Varied
{
    [Table("AttributeValues")]
    [Index(nameof(AttributeId), nameof(Value), IsUnique = true)]
    public class AttributeValuesModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Giá trị thuộc tính là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Giá trị thuộc tính không được vượt quá 255 ký tự")]
        public string Value { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //
        [Required]
        public int AttributeId { get; set; }
        [ForeignKey(nameof(AttributeId))]
        [JsonIgnore]
        public virtual AttributeModel? Attribute { get; set; }

        public virtual ICollection<VariantAttributeValuesModel> VariantAttributeValues {  get; set; } = new List<VariantAttributeValuesModel>();
    }
}
