
using clothing_be.Data;
using clothing_be.DTO.production;
using clothing_be.DTO.production.category;
using clothing_be.DTO.production.tag;
using clothing_be.Models.Others;
using clothing_be.Models.productions;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class DiscountModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;
    private readonly IImageUploadService _imageUploadService;

    public DiscountModelsController(MyDbContextApplication context, IImageUploadService imageUploadService)
    {
        _context = context;
        _imageUploadService = imageUploadService;
    }

    // GET: DISCOUNTMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetAllDiscount()
    {
        var discounts = await _context.Discounts
            .Include(d => d.Image)
            .Include(d => d.Status)
            .Include(d => d.Products)
            .ToListAsync();

        var dto = discounts.Select(dis => new DiscountDTO
        {
            Id = dis.Id,
            Title = dis.Title,
            Description = dis.Description,
            Codes = dis.Codes,
            Percentage = dis.Percentage,
            StartAt = dis.StartAt,
            Duration = dis.Duration,
            ExpireAt = dis.ExpireAt,
            StatusName = dis.Status?.Name,
            ImageURL = dis.Image?.URL,
            Products = dis.Products
                .OrderBy(p => p.CreatedAt)
                .Select(p => new ProductCardDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    BasePrice = p.BasePrice
                }).ToList()
        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<DiscountDTO>>> GetById(int id)
    {
        var dis = await _context.Discounts
            .Include(d => d.Image)
            .Include(d => d.Status)
            .Include(d => d.Products)
            .FirstOrDefaultAsync(d => d.Id == id);

        var dto = new DiscountDTO
        {
            Id = dis.Id,
            Title = dis.Title,
            Description = dis.Description,
            Codes = dis.Codes,
            Percentage = dis.Percentage,
            StartAt = dis.StartAt,
            Duration = dis.Duration,
            ExpireAt = dis.ExpireAt,
            StatusName = dis.Status?.Name,
            ImageURL = dis.Image?.URL,
            Products = dis.Products
                .OrderBy(p => p.CreatedAt)
                .Select(p => new ProductCardDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    BasePrice = p.BasePrice
                }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CreateDiscountDTO>> CreateDiscount([FromForm] CreateDiscountDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "Discounts").FirstOrDefaultAsync();
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

            _context.Images.Add(image);
        }

        var newProduct = new List<ProductModel>();
        if (dto.ProductIds.Any())
        {
            newProduct = await _context.Products
                .Where(p => dto.ProductIds.Contains(p.Id))
                .ToListAsync();

            var missingIds = dto.ProductIds.Except(newProduct.Select(p => p.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest($"Các proidyct sau không tồn tại: {string.Join(", ", missingIds)}");
            }
        }

        var dis = new DiscountModel
        {
            Title = dto.Title,
            Description = dto.Description,
            Codes = dto.Codes,
            Percentage = dto.Percentage,
            StartAt = dto.StartAt,
            Duration = dto.Duration,
            Image = image,
            StatusId = status.Id
        };

        _context.Discounts.Add(dis);
        await _context.SaveChangesAsync();

        foreach (var pro in newProduct)
        {
            pro.DiscountId = dis.Id;
        }
        await _context.SaveChangesAsync();

        var resultdto = new DiscountCardsDTO
        {
            Id = dis.Id,
            Title = dis.Title,
            Description = dis.Description
        };

        return CreatedAtAction(nameof(GetById), new { id = dis.Id }, resultdto);

    }
}
