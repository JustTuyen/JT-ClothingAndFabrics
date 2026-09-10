using clothing_be.Models.Others;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.trading
{
    [Table("PaymentMethods")]
    public class PaymentMethodModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phương pháp là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên phương pháp không được vượt quá 255 ký tự")]
        public string Method { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //
        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        //
        public virtual ICollection<OrderModel> Orders {  get; set; } = new List<OrderModel>();
        public virtual ICollection<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();
    }
}
