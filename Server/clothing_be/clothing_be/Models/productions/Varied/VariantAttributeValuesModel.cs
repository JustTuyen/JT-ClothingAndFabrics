using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_be.Models.productions.Varied
{
    [Table("VariantAttributeValues")]
    [Index(nameof(VariationId), nameof(AttributeValueId), IsUnique = true)]
    public class VariantAttributeValuesModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AttributeValueId { get; set; }
        [ForeignKey(nameof(AttributeValueId))]
        public virtual AttributeValuesModel? AttributeValue {  get; set; }

        [Required]
        public int VariationId { get; set; }
        [ForeignKey(nameof(VariationId))]
        public virtual VariationModel? Variation { get; set; }
    }
}
