using clothing_be.Models.customers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace clothing_be.Models.trading
{
    [Table("Carts")]
    public class CartModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        //
        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public virtual UserModel? User { get; set; }

        public virtual ICollection<CartItemsModel> CartItems { get; set; } = new List<CartItemsModel>();
    }
}
