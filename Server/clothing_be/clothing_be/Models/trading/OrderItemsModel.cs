using clothing_be.Models.productions.Varied;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.trading
{
    [Table("OrderItems")]
    [Index(nameof(OrderId))]
    [Index(nameof(OrderId), nameof(VariationId), IsUnique = true)]
    public class OrderItemsModel
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(2000, ErrorMessage = "Độ dài tên danh mục không được vượt quá 2000 ký tự")]
        public string? Note { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng không được nhỏ hơn 1")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //
        [Required]
        public int VariationId { get; set; }
        [ForeignKey(nameof(VariationId))]
        [JsonIgnore]
        public virtual VariationModel? Variation { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [JsonIgnore]
        public virtual OrderModel? Order { get; set; }
    }
}
