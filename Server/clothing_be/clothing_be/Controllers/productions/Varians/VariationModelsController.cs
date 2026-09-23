
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
                .ThenInclude(vv => vv.AttributeValue)
            .Include(v => v.Product)
            .OrderBy(v => v.CreatedAt)
            .ToListAsync();

        var dto = variants.Select(variant => new VariationDTO
        {
            Id = variant.Id,
            AddPrice = variant.AddPrice,
            StockQuantity = variant.StockQuantity,
            Sku = variant.Sku,
            ImageURL = variant.Image?.URL,
            ProductName = variant.Product?.Name,
            StatusName = variant.Status?.Name,
            VariantAttributes = variant.VariantAttributeValues
                .Select(c => new VariantAttributeValuesDTO
                {
                    Id = c.Id,
                    AttributeValues = c.AttributeValue.Value
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
                .ThenInclude(vv => vv.AttributeValue)
            .Include(v => v.Product)
            .FirstOrDefaultAsync(v => v.Id == id);

        if (variant == null) { return NotFound(); }

        var dto = new VariationDTO
        {
            Id = variant.Id,
            AddPrice = variant.AddPrice,
            StockQuantity = variant.StockQuantity,
            Sku = variant.Sku,
            ImageURL = variant.Image?.URL,
            ProductName = variant.Product?.Name,
            StatusName = variant.Status?.Name,
            VariantAttributes = variant.VariantAttributeValues
                .Select(c => new VariantAttributeValuesDTO
                {
                    Id = c.Id,
                    AttributeValues = c.AttributeValue.Value
                }).ToList()
        };

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VariationDTO>> UpdateVariation(int id, [FromForm] UpdateVariationDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var var = await _context.Variations.FirstOrDefaultAsync(v => v.Id == id);
        if (var == null) return NotFound("Variantion không tồn tại.");

        var pro = await _context.Products.FirstOrDefaultAsync(v => v.Id == dto.ProductId);
        if (pro == null) return NotFound("Variantion không tồn tại.");

        var sta = await _context.Statuses.FirstOrDefaultAsync(st => st.Id == dto.StatusId);
        if (sta == null) { return BadRequest("Status ko ton tai"); }
        if (sta.Type != "Variations") { return BadRequest("Status ko danh cho categoy!"); }

        var skuExists = await _context.Variations
            .AnyAsync(v => v.Sku == dto.Sku && v.Id != id);   
        if (skuExists)
            return Conflict($"SKU '{dto.Sku}' đã được dùng bởi variation khác.");

        if (dto.AttributeValueIds != null && dto.AttributeValueIds.Any())
        {
            var existingValues = await _context.AttributeValues
                .Where(v => dto.AttributeValueIds.Contains(v.Id))
                .ToListAsync();

            var missingIds = dto.AttributeValueIds.Except(existingValues.Select(v => v.Id)).ToList();
            if (missingIds.Any())
                return BadRequest($"Invalid ValueIds: {string.Join(", ", missingIds)}");

            var.VariantAttributeValues.Clear();
            var.VariantAttributeValues = existingValues
                .Select(v => new VariantAttributeValuesModel { AttributeValueId = v.Id })
                .ToList();
        }

        if (dto.Image != null)
        {
            var uploadResult = await _imageUploadService.UploadAsync(dto.Image);
            var newImage = new ImageModel
            {
                URL = uploadResult.Url,
                PublicId = uploadResult.Key,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Type = dto.Image.ContentType,
                IsThumbnail = false,
            };

            _context.Images.Add(newImage);

            if (var.Image != null)
            {
                await _imageUploadService.DeleteAsync(var.Image.PublicId);
                _context.Images.Remove(var.Image);
            }

            var.Image = newImage;
        }

        var.AddPrice = dto.AddPrice;
        var.StockQuantity = dto.StockQuantity;
        var.Sku = dto.Sku;
        var.StatusId = dto.StatusId;
        var.ProductId = dto.ProductId;

        await _context.SaveChangesAsync();

        var resultdto = new VariationDTO
        {
            Id = var.Id,
            AddPrice = var.AddPrice,
            StockQuantity = var.StockQuantity
        };

        return Ok(resultdto);

    }

    [HttpPost]
    public async Task<ActionResult<VariationDTO>> CreateVariation([FromForm] CreateVariationDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var pro = await _context.Products
            .Include(c => c.ImageGalleries)
            .Include(c => c.Status)
            .FirstOrDefaultAsync(c => c.Id == dto.ProductId);
        if (pro == null) { return BadRequest("Product is not found"); }

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "Variations").FirstOrDefaultAsync();
        if (status == null) { return BadRequest("Status is not found"); }
        
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

            var nextDisplayOrder = pro.ImageGalleries.Any() ? pro.ImageGalleries.Max(p => p.DisplayOrder) + 1 : 0;
            var proImgGallary = await _context.ImageGalleries.Where(i => i.ProductId == dto.ProductId).FirstOrDefaultAsync();
            pro.ImageGalleries.Add(new ImageGalleryModel
            {
                Image = image,
                DisplayOrder = nextDisplayOrder
            });
        }

        var AttributeValues = new List<VariantAttributeValuesModel>();
        if(dto.AttributeValueIds!=null && dto.AttributeValueIds.Any())
        {
            foreach(var valueId in dto.AttributeValueIds)
            {
                var value = await _context.AttributeValues.FirstOrDefaultAsync(va => va.Id == valueId);
                if(value == null) { return BadRequest($"Invalid ValueId: {valueId}"); }
                AttributeValues.Add(new VariantAttributeValuesModel
                {
                    AttributeValueId = valueId
                });
            }
        }



        var varian = new VariationModel
        {
            AddPrice = dto.AddPrice,
            StockQuantity = dto.StockQuantity,
            Sku = dto.Sku,
            StatusId = status.Id,
            ProductId = dto.ProductId,
            Image = image,
            VariantAttributeValues = AttributeValues,
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
            StatusName = status.Name,
            ProductName = pro.Name,
            ImageURL = image?.URL,
        };

        return CreatedAtAction(nameof(GetById), new { id = varian.Id }, resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<VariationModel>> DeleteVariation(int id)
    {
        var var = await _context.Variations
            .FirstOrDefaultAsync(va => va.Id == id);

        if (var == null) { return BadRequest("Variation with this id cant found"); }
        _context.Variations.Remove(var);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{productId}/variations/{variationId}")]
    public async Task<IActionResult> DeleteProductVariation(int productId, int variationId)
    {
        var var = await _context.Variations.FirstOrDefaultAsync(v => v.Id == variationId && v.ProductId == productId);
        if (var == null) return NotFound();

        _context.Variations.Remove(var);
        await _context.SaveChangesAsync();

        return NoContent();
    }



}
