using clothing_be.DTO.production.category;

namespace clothing_be.DTO.production
{
    public class DiscountDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Codes { get; set; }
        public decimal Percentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime StartAt { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ExpireAt { get; set; }
        public string StatusName { get; set; }
        public string ImageURL { get; set; }
        public List<ProductCardDTO> Products { get; set; } = new();
    }

    public class DiscountCardsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public string ImageURL { get; set; }
    }

    public class CreateDiscountDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Codes { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartAt { get; set; }
        public TimeSpan Duration { get; set; }
        public IFormFile? Image { get; set; }
        public int StatusId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }
}
