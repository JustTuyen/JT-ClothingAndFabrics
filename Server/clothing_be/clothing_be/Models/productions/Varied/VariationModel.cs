using clothing_be.Models.Others;
using clothing_be.Models.trading;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.productions.Varied
{
    [Table("Variations")]
    [Index(nameof(Sku), IsUnique = true)]
    [Index(nameof(ProductId))]
    public class VariationModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Giá thêm là bắt buộc")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá thêm không được nhỏ hơn 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AddPrice { get; set; }

        [Required(ErrorMessage = "Số lượng là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được nhỏ hơn 0")]
        public int StockQuantity { get; set; }

        [Required]
        [MaxLength(100)]
        public string Sku { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //
        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public int? ImageId { get; set; }
        [ForeignKey(nameof(ImageId))]
        [JsonIgnore]
        public virtual ImageModel? Image { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        [JsonIgnore]
        public virtual ProductModel? Product { get; set; }

        public virtual ICollection<VariantAttributeValuesModel> VariantAttributeValues { get; set; } = new List<VariantAttributeValuesModel>();
        public virtual ICollection<CartItemsModel> CartItems { get; set; } = new List<CartItemsModel>();
        public virtual ICollection<OrderItemsModel> OrderItems { get; set; } = new List<OrderItemsModel>();
        public virtual ICollection<InvoiceItemsModel> InvoiceItems { get; set; } = new List<InvoiceItemsModel>();

    }
}
