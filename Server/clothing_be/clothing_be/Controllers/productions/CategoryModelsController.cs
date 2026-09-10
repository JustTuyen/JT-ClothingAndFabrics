using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using clothing_be.Models.productions;
using clothing_be.Data;
using clothing_be.DTO.production.category;
using clothing_be.Services.Media;
using clothing_be.Models.Others;
//FOR API 
[ApiController]
[Route("api/[controller]")]
public class CategoryModelsController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;
    private readonly MyDbContextApplication _context;

    public CategoryModelsController(MyDbContextApplication context, IImageUploadService imageUploadService)
    {
        _imageUploadService = imageUploadService;
        _context = context;
    }

    // GET: CATEGORYMODELS
    [HttpGet("{id}")]
    public async Task<ActionResult<ListingCategoryDTO>> GetCategory(int id)
    {
        var cat = await _context.Categories
            .Include(c=>c.SubCategories)
            .FirstOrDefaultAsync(c=>c.Id== id);

        if(cat== null) return NotFound();

        return Ok(cat);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategory()
    {
        var cats = await _context.Categories
            .Include(x => x.Image)
            .Include(x => x.Status)
            .Include(x => x.SubCategories)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        var dto = cats.Select(cat => new CategoryDTO
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            CreatedAt = cat.CreatedAt,
            UpdatedAt = cat.UpdatedAt,
            ImageURL = cat.Image?.URL,
            StatusName = cat.Status?.Name,
            SubCategories = cat.SubCategories
                .OrderBy(sub => sub.CreatedAt)
                .Select(sub => new ListingSubCategoryDTO
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    Slug = sub.Slug
                }).ToList()
            
        }).ToList();   

        return Ok(dto);
    }

    [HttpGet("listing")]
    public async Task<ActionResult<IEnumerable<ListingCategoryDTO>>> GetCategoriesListing()
    {
        var dto = await _context.Categories
        .Where(c => c.Status!.Type == "Category" && c.Status!.Name == "Active")
        .OrderBy(c => c.CreatedAt)
        .Select(cat => new ListingCategoryDTO
        {
            Id = cat.Id,
            Name = cat.Name,
            SubCategories = cat.SubCategories
                .OrderBy(sub => sub.CreatedAt)
                .Select(sub => new ListingSubCategoryDTO
                {
                    Id = sub.Id,
                    Name = sub.Name,
                    Slug = sub.Slug,
                    //Category = sub.Category.Name
                }).ToList()
        })
        .ToListAsync();

        return Ok(dto);
    }


    [HttpGet("card")]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetCategoriesCards()
    {
        var dto = await _context.Categories
        .Include(c => c.Image)
        .Where(c => c.Status!.Type == "Category" && c.Status!.Name == "Active")
        .OrderBy(c => c.CreatedAt)
        .Select(cat => new CardCategoryDTO
        {
            Id = cat.Id,
            Name = cat.Name,
            ImageURL = cat.Image.URL
        })
        .ToListAsync();

        return Ok(dto);
    }


    [HttpPost]
    public async Task<ActionResult<CategoryModel>> CreateCategory([FromForm] CreateCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);
      
        ImageModel? image = null;
        if(dto.Image != null)
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

        var category = new CategoryModel
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Image = image,
            StatusId = dto.StatusId
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        var catdto = new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, catdto);
    }
}
