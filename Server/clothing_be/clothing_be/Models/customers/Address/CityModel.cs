using clothing_be.Models.Others;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.customers.Address
{
    [Table("Cities")]
    public class CityModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên thành phố là bắt buộc")]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        //[ForeignKey]
        [Required(ErrorMessage = "Status ID là bắt buộc")]
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        [JsonIgnore]
        public virtual StatusModel? Status { get; set; }
        public virtual ICollection<AddressModel> Addresses { get; set; } = new List<AddressModel>();
        public virtual ICollection<DistrictModel> Districts { get; set; } = new List<DistrictModel>();
    }
}
