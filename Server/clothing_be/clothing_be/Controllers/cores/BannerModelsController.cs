
using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production.tag;
using clothing_be.Models.Others;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class BannerModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;
    private readonly IImageUploadService _imageUploadService;

    public BannerModelsController(MyDbContextApplication context, IImageUploadService imageUploadService)
    {
        _context = context;
        _imageUploadService = imageUploadService;
    }

    // GET: BANNERMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BannerDTO>>> GetAllBanner()
    {
        var banners = await _context.Banners
            .Include(b => b.Image)
            .Include(b => b.Status)
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        var dto = banners.Select(banner => new BannerDTO
        {
            Id = banner.Id,
            Name = banner.Name,
            Description = banner.Description,
            TargetUrl = banner.TargetUrl,
            CreatedAt = banner.CreatedAt,
            DisplayOrder = banner.DisplayOrder,
            UpdatedAt = banner.UpdatedAt,
            ImageURL = banner.Image?.URL,
            StatusName = banner.Status?.Name
        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<BannerDTO>>> GetById(int id)
    {
        var banner = await _context.Banners
            .Include(v => v.Status)
            .Include(v => v.Image)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (banner == null) return BadRequest("No banner with id is found");
        var dto = new BannerDTO
        {
            Id = banner.Id,
            Name = banner.Name,
            Description = banner.Description,
            TargetUrl = banner.TargetUrl,
            CreatedAt = banner.CreatedAt,
            DisplayOrder = banner.DisplayOrder,
            UpdatedAt = banner.UpdatedAt,
            ImageURL = banner.Image?.URL,
            StatusName = banner.Status?.Name
        };

        return Ok(dto);
    }

    [HttpGet("listing")]
    public async Task<ActionResult<IEnumerable<BannerListingDTO>>> ListingBanner()
    {
        var banners = await _context.Banners
            .Include(b => b.Image)
            .Include(b => b.Status)
            .Where(b => b.Status.Name == "Active")
            .OrderBy(b => b.DisplayOrder)
            .ToListAsync();

        var dto = banners.Select(banner => new BannerListingDTO
        {
            Id = banner.Id,
            Name = banner.Name,
            Description = banner.Description,
            ImageURL = banner.Image?.URL,
            TargetUrl = banner.TargetUrl
        }).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<CreateBannerDTO>> CreateBanner([FromForm] CreateBannerDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sta = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == dto.StatusId);
        if (sta == null) { return BadRequest("Status is not found"); }
        if (sta.Name != "Active") { return BadRequest("wrong status name or type"); }
        if (sta.Type != "Banners") { return BadRequest("wrong status type"); }

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

        var banner = new BannerModel
        {
            Name = dto.Name,
            Description = dto.Description,
            Image = image,
            StatusId = dto.StatusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            DisplayOrder = dto.DisplayOrder,
            TargetUrl = dto.TargetUrl
        };

        _context.Banners.Add(banner);
        await _context.SaveChangesAsync();

        var resultdto = new BannerListingDTO
        {
            Id = banner.Id,
            Name = banner.Name,
            Description = banner.Description,
        };
        return CreatedAtAction(nameof(GetById), new { id = banner.Id }, resultdto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Deletebanner(int id)
    {
        var banner = await _context.Banners
            .Include(v => v.Status)
            .Include(v => v.Image)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (banner == null) return BadRequest("No banner with id is found");
        _context.Banners.Remove(banner);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateBannerDTO>> UpdateBanner(int id, [FromForm] UpdateBannerDTO dto)
    {
        var banner = await _context.Banners
            .Include(v => v.Status)
            .Include(v => v.Image)
            .FirstOrDefaultAsync(v => v.Id == id);
        if (banner == null) return BadRequest("No banner with id is found");

        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sta = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == dto.StatusId);
        if (sta == null) { return BadRequest("Status is not found"); }
        if (sta.Type != "Banner") { return BadRequest("wrong status type"); }

        if (dto.Image != null)
        {
            var uploadResult = await _imageUploadService.UploadAsync(dto.Image);
            var newImg = new ImageModel
            {
                URL = uploadResult.Url,
                PublicId = uploadResult.Key,
                Width = uploadResult.Width,
                Height = uploadResult.Height,
                Type = dto.Image.ContentType,
                CreatedAt = DateTime.UtcNow,
                IsThumbnail = false,
            };

            _context.Images.Add(newImg);

            if (banner.Image != null)
            {
                await _imageUploadService.DeleteAsync(banner.Image.PublicId);
                _context.Images.Remove(banner.Image);
            }

            banner.Image = newImg;
        }

        if (dto.Name != null) { banner.Name = dto.Name; }
        if (dto.Description != null) { banner.Description = dto.Description; }
        if (dto.StatusId != null) { banner.StatusId = dto.StatusId; }
        if (dto.DisplayOrder != null) { banner.DisplayOrder = dto.DisplayOrder; }
        if (dto.TargetUrl != null) { banner.TargetUrl = dto.TargetUrl; }
        banner.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        var resultdto = new BannerListingDTO
        {
            Id = banner.Id,
            Name = banner.Name,
            Description = banner.Description,
        };
        return Ok(resultdto);
    }
}
