using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production.category;
using clothing_be.Models.productions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class SubCategoryModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public SubCategoryModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: SUBCATEGORYMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubCategoryModel>>> GetAllSubCategories()
    {
        var subs = await _context.SubCategories
            .Include(x => x.Category)
            .Include(x => x.Status)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        var dto = subs.Select(sub => new ListingSubCategoryDTO
        {
            Id = sub.Id,
            Name = sub.Name,
            Slug = sub.Slug,
            //Category = sub.Category.Name,
        }).ToList();

        return Ok(dto);
        
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<IEnumerable<SubCategoryModel>>> GetSubCategory(int id)
    {
        var sub = await _context.SubCategories
            .Include(x => x.Category)
            .Where(c => c.Status!.Type == "Category" && c.Status!.Name == "Active")
            .FirstOrDefaultAsync(x => x.Id == id);

        return Ok(sub);
    }

    [HttpPost]
    public async Task <ActionResult<SubCategoryModel>> CreateSubCategory([FromBody] CreateSubCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cat = await _context.Categories
            .Where(c => c.Status!.Type == "Category" && c.Status!.Name == "Active")
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
        if (cat == null) return BadRequest("the category is either not exist or not active");

        var sub = new SubCategoryModel
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Slug = dto.Slug,
            StatusId = dto.StatusId,
            CategoryId = dto.CategoryId
        };

        _context.SubCategories.Add(sub);
        await _context.SaveChangesAsync();

        var subdto = new SubCategoryDTO
        {
            Id = sub.Id,
            Name = sub.Name,
            Description = sub.Description,
        };

        return CreatedAtAction(nameof(GetSubCategory), new { id = sub.Id }, subdto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubCategoryModel>> PutSubCategory(int id, [FromBody] UpdateSubCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto is null or missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sub = await _context.SubCategories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (sub == null) return NotFound();

        var cat = await _context.Categories
            .Where(c => c.Status!.Type == "Category" && c.Status!.Name == "Active")
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
        if (cat == null) return BadRequest("the category is either not exist or not active");

        sub.Name = dto.Name;
        sub.Description = dto.Description;
        sub.Slug = dto.Slug;
        sub.StatusId = dto.StatusId;
        sub.CategoryId = dto.CategoryId;
        sub.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedSub = await _context.SubCategories
            .FirstOrDefaultAsync(c => c.Id == id);

        var updatedto = new SubCategoryDTO
        {
            Id = id,
            Name = updatedSub.Name,
            Slug = updatedSub.Slug,
            Description = updatedSub.Description,
            CategoryId = dto.CategoryId,
        };

        return Ok(updatedto);
    }
    
}
