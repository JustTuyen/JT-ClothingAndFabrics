using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.customers.Address
{
    [Table("Wards")]
    [Index(nameof(DistrictId))]
    public class WardModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên phường/xã là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên phường/xã không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //[ForeignKey]
        [Required(ErrorMessage = "District ID là bắt buộc")]
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        [JsonIgnore]
        public virtual DistrictModel? District { get; set; }

        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public virtual ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();

    }
}

