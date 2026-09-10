using clothing_be.Models.customers.Address;
using clothing_be.Models.Others;
using clothing_be.Models.productions.Activity;
using clothing_be.Models.trading;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.customers
{
    [Table("Users")]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ")]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài mật khẩu không được vượt quá 255 ký tự")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Độ dài tên không được vượt quá 255 ký tự")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Độ dài họ không được vượt quá 255 ký tự")]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(25, ErrorMessage = "Độ dài số điện thoại không được vượt quá 25 ký tự")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(25, ErrorMessage = "Độ dài giới tính không được vượt quá 25 ký tự")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role là bắt buộc")]
        [MaxLength(50, ErrorMessage = "Độ dài role không được vượt quá 50 ký tự")]
        public string Role { get; set; } = string.Empty;
        public DateTime? BirthDay { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property 
        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
        public virtual ICollection<ActivityLogModel> ActivityLogs { get; set; } = new List<ActivityLogModel>();
        public virtual ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
        public virtual ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();
        public virtual ICollection<FavoritedModel> Favorites { get; set; } = new List<FavoritedModel>();
        public virtual ICollection<InvoiceModel> Invoices { get; set; } = new List<InvoiceModel>();
        public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();


    }
}
