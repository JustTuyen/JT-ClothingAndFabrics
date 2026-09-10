using clothing_be.Models.customers;
using clothing_be.Models.customers.Address;
using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.trading
{
    [Table("Invoices")]
    [Index(nameof(UserId))]
    [Index(nameof(OrderId), IsUnique = true)]
    public class InvoiceModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string OrderCode { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Độ dài tên không được vượt quá 255 ký tự")]
        public string? FirstName { get; set; }

        [MaxLength(255, ErrorMessage = "Độ dài họ không được vượt quá 255 ký tự")]
        public string? LastName { get; set; }

        [MaxLength(25, ErrorMessage = "Độ dài số điện thoại không được vượt quá 25 ký tự")]
        public string? PhoneNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingFee { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [MaxLength(1500, ErrorMessage = "Độ dài note không được vượt quá 1500 ký tự")]
        public string? Note { get; set; }

        public DateTime? PaidAt { get; set; }
        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(500)]
        public string ShippingAddressSnapshot { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Method { get; set; } = string.Empty;
        //
        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        [Required]
        public int AddressId { get; set; }
        [ForeignKey(nameof(AddressId))]
        [JsonIgnore]
        public virtual AddressModel? Address { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        [JsonIgnore]
        public virtual OrderModel? Order { get; set; }

        [Required]
        public int MethodId { get; set; }
        [ForeignKey(nameof(MethodId))]
        [JsonIgnore]
        public virtual PaymentMethodModel? PaymentMethod { get; set; }

        public virtual ICollection<InvoiceItemsModel> InvoiceItems { get; set; } = new List<InvoiceItemsModel>();
    }
}

