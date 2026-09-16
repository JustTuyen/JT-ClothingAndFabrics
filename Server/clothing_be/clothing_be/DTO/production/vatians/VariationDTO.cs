using clothing_be.DTO.production.category;
using clothing_be.Models.productions.Varied;

namespace clothing_be.DTO.production.vatians
{
    public class VariationDTO
    {
        public int Id { get; set; }
        public decimal AddPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string StatusName { get; set; }
        public string? ImageURL { get; set; }
        public string? ProductName { get; set; }
        public List<VariantAttributeValuesDTO> VariantAttributeList { get; set; } = new();
    }

    public class ListingVariationDTO
    {
        public int Id { get; set; }
        public decimal AddPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string StatusName { get; set; }
        public string? ImageURL { get; set; }
    }

    public class CreateVariationDTO
    {
        public decimal AddPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public IFormFile? Image { get; set; }
        public int ProductId { get; set; }
    }
    public class VariantAttributeValuesDTO 
    {
        public int Id { get; set; }
        public int AttributeValueId { get; set; }
        public int VariationId { get; set; }
    }


    public class CreateVariantAttributeValuesDTO
    {
        public int AttributeValueId { get; set; }
        public int VariationId { get; set; }
    }

}
