
using clothing_be.Controllers.cores;
using clothing_be.Data;
using clothing_be.DTO.production;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.productions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class ProductModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public ProductModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: PRODUCTMODELS
    [HttpGet]
    public async Task <ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Status)
            .Include(p => p.Discount)
            .Include(p => p.ImageGalleries)
            .Include(p => p.SubCategory)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();

        var dto = products.Select(product => new MenuProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            BasePrice = product.BasePrice,
            DiscountPercentage = product.Discount?.Percentage,
            StatusName = product.Status?.Name,
            ImageGalleries = product.ImageGalleries
                .OrderBy(ig => ig.DisplayOrder)
                .Select( ig => new MiniImageGalleryDTO
                {
                    Id = ig.Id,
                    ImageURL = ig.Image?.URL,
                    DisplayOrder = ig.DisplayOrder
                }).ToList(),

            Variations = product.Variations
                .OrderBy(pv => pv.CreatedAt)
                .Select(pv => new ListingVariationDTO
                {
                    Id = pv.Id,
                    Sku = pv.Sku,
                    StockQuantity = pv.StockQuantity,
                    AddPrice = pv.AddPrice,
                    StatusName = pv.Status?.Name,
                    ImageURL = pv.Image?.URL

                }).ToList()
        }).ToList();

        return Ok(dto);

        
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Status)
            .Include(p => p.Discount)
            .Include(p => p.ImageGalleries)
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(p => p.Id == id);

        if(product == null){ return NotFound(); }

        var dto = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BasePrice = product.BasePrice,
            DiscountPercentage = product.Discount?.Percentage,
            SubCategoryName = product.SubCategory?.Name,
            StatusName = product.Status?.Name,
            ImageGalleries = product.ImageGalleries
                .OrderBy(ig => ig.DisplayOrder)
                .Select(ig => new MiniImageGalleryDTO
                {
                    Id = ig.Id,
                    ImageURL = ig.Image?.URL,
                    DisplayOrder = ig.DisplayOrder
                }).ToList(),

            Variations = product.Variations
                .OrderBy(pv => pv.CreatedAt)
                .Select(pv => new ListingVariationDTO
                {
                    Id = pv.Id,
                    Sku = pv.Sku,
                    StockQuantity = pv.StockQuantity,
                    AddPrice = pv.AddPrice,
                    StatusName = pv.Status?.Name,
                    ImageURL = pv.Image?.URL

                }).ToList()
        };

        return Ok(dto);

    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> CreateProduct([FromForm] CreateProductDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sta = await _context.Statuses.FirstOrDefaultAsync(st => st.Id == dto.StatusId);
        if(sta == null) { return BadRequest("This status is missing or doesnt exist!"); }
        if(sta.Name != "Active" || sta.Type != "Product") { return BadRequest("This status name or type is mismatch to product!"); }

        var subcat = await _context.SubCategories.Include(st=> st.Status).FirstOrDefaultAsync(st => st.Id == dto.SubCategoryId);
        if (subcat == null) { return BadRequest("This SubCategories is missing or doesnt exist!"); }
        if (subcat.Status?.Name != "Active" || subcat.Status.Type != "SubCategory") { return BadRequest("This subcategories is closed or not for product"); }

        var product = new ProductModel
        {
            Name = dto.Name,
            Description = dto.Description,
            BasePrice = dto.BasePrice,
            SubCategoryId = dto.SubCategoryId,
            StatusId = dto.StatusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var resultDto = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BasePrice = product.BasePrice,
            //SubCategoryName = product.SubCategory.Name,
            //StatusName = product.Status.Name
        };

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, resultDto);
    }
}
