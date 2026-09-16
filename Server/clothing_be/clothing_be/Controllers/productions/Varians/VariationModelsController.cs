
using clothing_be.Data;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.Others;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Varied;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

[ApiController]
[Route("api/[controller]")]

public class VariationModelsController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;
    private readonly MyDbContextApplication _context;

    public VariationModelsController(MyDbContextApplication context, IImageUploadService imageUploadService)
    {
        _imageUploadService = imageUploadService;
        _context = context;
    }

    // GET: VARIATIONMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VariationDTO>>> GetAll()
    {
        var variants = await _context.Variations
            .Include(v => v.Image)
            .Include(v => v.Status)
            .Include(v => v.VariantAttributeValues)
            .Include(v => v.Product)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();

        var dto = variants.Select(variant => new VariationDTO
        {
            Id = variant.Id,
            AddPrice = variant.AddPrice,
            StockQuantity = variant.StockQuantity,
            Sku = variant.Sku,
            ImageURL = variant.Image.URL,
            ProductName = variant.Product.Name,
            StatusName = variant.Status.Name,
            VariantAttributeList = variant.VariantAttributeValues
                .Select(va => new VariantAttributeValuesDTO
                {
                    Id = va.Id,
                    AttributeValueId = va.AttributeValueId,
                    VariationId = va.VariationId
                }).ToList()

        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VariationDTO>> GetById(int id)
    {
        var variant = await _context.Variations
            .Include(v => v.Image)
            .Include(v => v.Status)
            .Include(v => v.VariantAttributeValues)
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (variant == null) { return NotFound(); }

        var dto = new VariationDTO
        {
            Id = variant.Id,
            AddPrice = variant.AddPrice,
            StockQuantity = variant.StockQuantity,
            Sku = variant.Sku,
            ImageURL = variant.Image.URL,
            ProductName = variant.Product.Name,
            StatusName = variant.Status.Name,
            VariantAttributeList = variant.VariantAttributeValues
                .Select(va => new VariantAttributeValuesDTO
                {
                    Id = va.Id,
                    AttributeValueId = va.AttributeValueId,
                    VariationId = va.VariationId
                }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<VariationDTO>> CreateVariation([FromForm] CreateVariationDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        ImageModel? image = null;
        if (dto.Image != null)
        {
            var uploadResult = await _imageUploadService.UploadAsync(dto.Image);
            image = new ImageModel
            {
                URL = uploadResult.Url,
                PublicId = uploadResult.Key,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Type = dto.Image.ContentType,
                CreatedAt = DateTime.UtcNow,
                IsThumbnail = false,
            };

            _context.Images.Add(image);
        }

        var pro = await _context.Products.Include(c => c.Status).FirstOrDefaultAsync(c => c.Id == dto.ProductId);
        if (pro == null) { return BadRequest("Product is not found"); }


        var sta = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == dto.StatusId);
        if (sta == null) { return BadRequest("Status is not found"); }
        if (sta.Name != "Active" || sta.Type != "Variantions") { return BadRequest("wrong status name or type"); }

        var varian = new VariationModel
        {
            AddPrice = dto.AddPrice,
            StockQuantity = dto.StockQuantity,
            Sku = dto.Sku,
            StatusId = dto.StatusId,
            ProductId = dto.ProductId,
            Image = image,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Variations.Add(varian);
        await _context.SaveChangesAsync();

        var resultDto = new VariationDTO
        {
            Id = varian.Id,
            Sku = varian.Sku,
            AddPrice = varian.AddPrice,
            StatusName = varian.Status.Name,
            ProductName = varian.Product.Name,
            ImageURL = varian.Image.URL,
        };

        return CreatedAtAction(nameof(GetById), new { id = varian.Id }, resultDto);
    }

}
