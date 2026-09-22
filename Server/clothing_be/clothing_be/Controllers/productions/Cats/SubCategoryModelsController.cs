using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production;
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
            .Include(x => x.Products)
                .ThenInclude(xx => xx.Status)
            .Include(x => x.Category)
            .Include(x => x.Status)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();

        var dto = subs.Select(sub => new ListingSubCategoryDTO
        {
            Id = sub.Id,
            Name = sub.Name,
            Slug = sub.Slug,
            StatusName = sub.Status?.Type,
            Products = sub.Products
                .OrderBy(x => x.ViewCount)
                .Select(x => new MiniProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    BasePrice = x.BasePrice,
                    StatusName = x.Status.Name

                }).ToList()

        }).ToList();

        return Ok(dto);
        
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<SubCategoryModel>> GetSubCategory(int id)
    {
        var sub = await _context.SubCategories
            .Include(x => x.Category)
            .Include(x => x.Status)
            .Include(x => x.Products)
                .ThenInclude(x => x.Status)
            .FirstOrDefaultAsync(x => x.Id == id);

        var dto = new SubCategoryDTO
        {
            Id = id,
            Name = sub.Name,
            Description = sub.Description,
            Slug = sub.Slug,
            StatusName = sub.Status.Name,
            CategoryName = sub.Category.Name,
            CreatedAt = sub.CreatedAt,
            UpdatedAt = sub.UpdatedAt,
            Products = sub.Products
                .OrderBy(x => x.ViewCount)
                .Select(x => new MiniProductDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    BasePrice = x.BasePrice,
                    StatusName = x.Status.Name

                }).ToList()
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task <ActionResult<SubCategoryModel>> CreateSubCategory([FromForm] CreateSubCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cat = await _context.Categories
            .Where(c => c.Status!.Type == "Categories" && c.Status!.Name == "Active")
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
        if (cat == null) return BadRequest("the category is either not exist or not active");

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "SubCategories").FirstOrDefaultAsync();
        if (status == null) { return BadRequest("Status is not found"); }

        //var status = await _context.Statuses
        //    .FirstOrDefaultAsync(s => s.Id == dto.StatusId);

        //if (status == null)
        //    return BadRequest("Status không tồn tại.");
        //if (status.Type != "SubCategories")
        //    return BadRequest("Status is not for SubCategory!");
        //if (status.Name != "Active") 
        //    return BadRequest("Status is not active!");

        //var newProducts = new List<ProductModel>();
        //if (dto.ProductIds.Any())
        //{
        //    newProducts = await _context.Products
        //        .Where(x => dto.ProductIds.Contains(x.Id))
        //        .ToListAsync();

        //    var missingIds = dto.ProductIds.Except(newProducts.Select(s => s.Id)).ToList();
        //    if (missingIds.Any())
        //    {
        //        return BadRequest($"Các Product sau không tồn tại: {string.Join(", ", missingIds)}");
        //    }
        //}

        var sub = new SubCategoryModel
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Slug = dto.Slug,
            StatusId = status.Id,
            CategoryId = dto.CategoryId,
            //Products = newProducts
        };

        _context.SubCategories.Add(sub);
        await _context.SaveChangesAsync();

        //foreach (var pro in newProducts)
        //{
        //    pro.SubCategoryId = sub.Id;
        //}
        //await _context.SaveChangesAsync();

        var resultDto = new SubCategoryDTO
        {
            Id = sub.Id,
            Name = sub.Name,
            Description = sub.Description,
            CategoryName = sub.Category?.Name,
            StatusName = sub.Status?.Name,
        };

        return CreatedAtAction(nameof(GetSubCategory), new { id = sub.Id }, resultDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubCategoryModel>> PutSubCategory(int id, [FromForm] UpdateSubCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto is null or missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sub = await _context.SubCategories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (sub == null) return NotFound();

        var cat = await _context.Categories
            .Where(c => c.Status!.Type == "Categories" && c.Status!.Name == "Active")
            .FirstOrDefaultAsync(c => c.Id == dto.CategoryId);
        if (cat == null) return BadRequest("the Category is either not exist or not active");

        var status = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Id == dto.StatusId);

        if (status == null)
            return BadRequest("Status không tồn tại.");

        if (status.Name != "Active") { return BadRequest("wrong status name or type"); }
        if (status.Type != "SubCategories") { return BadRequest("wrong status type"); }

        var newProducts = new List<ProductModel>();
        if (dto.ProductIds.Any())
        {
            newProducts = await _context.Products
                .Where(x => dto.ProductIds.Contains(x.Id))
                .ToListAsync();

            var missingIds = dto.ProductIds.Except(newProducts.Select(s => s.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest($"Các Product sau không tồn tại: {string.Join(", ", missingIds)}");
            }
        }

        sub.Name = dto.Name;
        sub.Description = dto.Description;
        sub.Slug = dto.Slug;
        sub.StatusId = dto.StatusId;
        sub.CategoryId = dto.CategoryId;
        sub.UpdatedAt = DateTime.UtcNow;
        sub.Products = newProducts;

        foreach (var pro in newProducts)
        {
            pro.SubCategoryId = sub.Id;
        }
        await _context.SaveChangesAsync();


        var updatedSub = await _context.SubCategories
            .FirstOrDefaultAsync(c => c.Id == id);

        var updatedto = new SubCategoryDTO
        {
            Id = id,
            Name = updatedSub.Name,
            Slug = updatedSub.Slug,
            Description = updatedSub.Description,
            CategoryName = updatedSub.Category?.Name,
            StatusName = updatedSub.Status?.Name,
        };

        return Ok(updatedto);
    }
    
}
