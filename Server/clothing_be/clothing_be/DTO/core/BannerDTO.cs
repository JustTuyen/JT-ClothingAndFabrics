namespace clothing_be.DTO.core
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TargetUrl { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImageURL { get; set; }
        public string StatusName { get; set; }
    }

    public class BannerListingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public string TargetUrl { get; set; } = string.Empty;
    }

    public class CreateBannerDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int StatusId { get; set; }
        public int DisplayOrder { get; set; }
        public string TargetUrl { get; set; }
    }

    public class UpdateBannerDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public int StatusId { get; set; }
        public int DisplayOrder { get; set; }
        public string TargetUrl { get; set; }
    }
}
