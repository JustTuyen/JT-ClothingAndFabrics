using clothing_be.Models.customers;
using clothing_be.Models.customers.Address;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Tagging;
using clothing_be.Models.productions.Varied;
using clothing_be.Models.trading;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace clothing_be.Models.Others
{
    [Table("Status")]
    [Index(nameof(Name), nameof(Type), IsUnique = true)]
    public class StatusModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Loại trạng thái là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên danh mục không được vượt quá 255 ký tự")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên trạng thái là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên danh mục không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<TagModel> Tags { get; set; } = new List<TagModel>();
        public virtual ICollection<DistrictModel> Districts { get; set; } = new List<DistrictModel>();
        public virtual ICollection<WardModel> Wards { get; set; } = new List<WardModel>();
        public virtual ICollection<CityModel> Cities { get; set; } = new List<CityModel>();
        public virtual ICollection<UserModel> Users { get; set; } = new List<UserModel>();
        public virtual ICollection<OrderModel> Orders { get; set; } = new List<OrderModel>();
        public virtual ICollection<VariationModel> Variations { get; set; } = new List<VariationModel>();
        public virtual ICollection<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public virtual ICollection<PaymentMethodModel> PaymentMethods { get; set; } = new List<PaymentMethodModel>();
        public virtual ICollection<ActivityLogModel> ActivityLogs { get; set; } = new List<ActivityLogModel>();

    }
}
