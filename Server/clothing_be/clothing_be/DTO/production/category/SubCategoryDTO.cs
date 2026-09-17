namespace clothing_be.DTO.production.category
{
    public class SubCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string CategoryName { get; set; }
        public string StatusName { get; set; }
        public List<MiniroductDTO> Products { get; set; } = new();
    }

    public class ListingSubCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;
        public List<MiniroductDTO> Products { get; set; } = new();
    }

    

    public class CreateSubCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }

    public class UpdateSubCategoryDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public List<int> ProductIds { get; set; } = new();
    }


}
