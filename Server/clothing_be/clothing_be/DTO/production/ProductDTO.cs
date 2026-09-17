using clothing_be.DTO.core;
using clothing_be.DTO.production.tag;
using clothing_be.DTO.production.vatians;

namespace clothing_be.DTO.production
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string SubCategoryName { get; set; }
        public string StatusName { get; set; }
        public List<ListingVariationDTO> Variations { get; set; } = new();
        public List<MiniImageGalleryDTO> ImageGalleries { get; set; } = new();
        public List<MiniProductTagDTO> Tags { get; set; } = new();
    }

    public class MenuProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string StatusName { get; set; }
        public List<ListingVariationDTO> Variations { get; set; } = new();
        public List<MiniImageGalleryDTO> ImageGalleries { get; set; } = new();
        public List<MiniProductTagDTO> Tags { get; set; } = new();
    }

    public class MiniroductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string StatusName { get; set; }

    }
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int SubCategoryId { get; set; }
        public int StatusId { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<int> TagIds { get; set; } = new();

    }


    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int StatusId { get; set; }
        public int SubCategoryId { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
    }
}
