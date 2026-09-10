using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.customers.Address
{
    [Table("Districts")]
    [Index(nameof(CityId))]
    public class DistrictModel
    {
       
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên quận/huyện là bắt buộc")]
        [MaxLength(255, ErrorMessage = "Độ dài tên quận/huyện không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        //[ForeignKey]
        [Required(ErrorMessage = "City ID là bắt buộc")]
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        [JsonIgnore]
        public virtual CityModel? City { get; set; }

        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }

        public virtual ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
        public virtual ICollection<WardModel> Wards { get; set; } = new List<WardModel>();
        
    }
}
