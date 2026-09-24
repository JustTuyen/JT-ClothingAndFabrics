using clothing_be.DTO.core;
using clothing_be.DTO.production.tag;
using clothing_be.DTO.production.vatians;

namespace clothing_be.DTO.production
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string SubCategoryName { get; set; }
        public string StatusName { get; set; }


        public List<ListingVariationDTO> Variations { get; set; } = new();
        public List<MiniImageGalleryDTO> ImageGalleries { get; set; } = new();
        //public List<MiniProductTagDTO> Tags { get; set; } = new();
        //public int ViewCount { get; set; }
    }

    public class MenuProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string StatusName { get; set; }
        public string ImageURL { get; set; }
        //public List<ListingVariationDTO> Variations { get; set; } = new();
        //public List<MiniImageGalleryDTO> ImageGalleries { get; set; } = new();
    }


    public class ProductModallDTO
    {
        public int Id { get; set; }
        //public string Slug { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        //public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string SubCategoryName { get; set; }
        public string StatusName { get; set; }
        public List<ListingVariationDTO> Variations { get; set; } = new();
        public List<MiniImageGalleryDTO> ImageGalleries { get; set; } = new();

        //public List<MiniProductTagDTO> Tags { get; set; } = new();
        //public int ViewCount { get; set; }
    }


    public class ProductCardDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
    }



    public class MiniProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public string StatusName { get; set; }
        public string ImageURL { get; set; }

    }
    public class CreateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int SubCategoryId { get; set; }
       //public int StatusId { get; set; }
        public string Slug { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<int> TagIds { get; set; } = new();

    }

    public class UpdateTagProductDTO
    {
        public List<int> TagIds { get; set; } = new();
    }

    public class UpdateProductDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public int StatusId { get; set; }
        public int SubCategoryId { get; set; }
        public string Slug { get; set; }
        public List<IFormFile> Images { get; set; } = new();
        public List<int> TagIds { get; set; } = new();
        public List<int> VariationsId { get; set; } = new();
    }

    public class ProductFilterDTO
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public bool? HasDiscount { get; set; }
        public int? MinViewCount { get; set; }

        // "CreatedAt" , "ViewCount" , "Price"
        public string? SortBy { get; set; } = "CreatedAt";  
        public bool SortDescending { get; set; } = true;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    
}
