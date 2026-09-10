using clothing_be.Models.trading;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace clothing_be.Models.customers.Address
{
    [Table("Addresses")]
    [Index(nameof(UserId))]
    public class AddressModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên người nhận không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên người nhận không quá 100 ký tự")]
        public string RecipientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại nhận hàng là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ chi tiết không được để trống")]
        [MaxLength(255, ErrorMessage = "Địa chỉ chi tiết (Số nhà, tên đường) không vượt quá 255 ký tự")]
        public string StreetAddress { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Độ ghi chú địa chỉ không được vượt quá 255 ký tự")]
        public string Note { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActived { get; set; } = false;

        //[ForeignKey]
        [Required(ErrorMessage = "City ID là bắt buộc")]
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        [JsonIgnore]
        public virtual CityModel? City { get; set; }

        [Required(ErrorMessage = "Discrit ID là bắt buộc")]
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [JsonIgnore]
        public virtual DistrictModel? District { get; set; }

        [Required(ErrorMessage = "Ward ID là bắt buộc")]
        public int WardId { get; set; }
        [ForeignKey(nameof(WardId))]
        [JsonIgnore]
        public virtual WardModel? Ward { get; set; }

        [Required(ErrorMessage = "User ID là bắt buộc")]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        public virtual ICollection<OrderModel> Orders {  get; set; } = new List<OrderModel>();
        public virtual ICollection<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();
    }
}
