using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using clothing_be.Models.productions;
using clothing_be.Data;
using clothing_be.DTO.production.category;
using clothing_be.Services.Media;
using clothing_be.Models.Others;
using System.Security.Cryptography.Xml;
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
            .Include(c => c.Status)
            .Include(c => c.Image)
            .Include(c=>c.SubCategories)
                .ThenInclude(c => c.Status)
            .FirstOrDefaultAsync(c=>c.Id== id);

        if(cat == null) return NotFound();

        var dto = new CategoryDTO
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            StatusName = cat.Status.Name,
            ImageURL = cat.Image.URL,
            CreatedAt = cat.CreatedAt,
            UpdatedAt = cat.UpdatedAt,
            SubCategories = cat.SubCategories
                .OrderBy(sc => sc.CreatedAt)
                .Select(sc => new ListingSubCategoryDTO
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Slug = sc.Slug,
                    StatusName = sc.Status?.Name

                }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategory()
    {
        var cats = await _context.Categories
            .Include(x => x.Image)
            .Include(x => x.Status)
            .Include(x => x.SubCategories)
                .ThenInclude(xx=> xx.Status)
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
                    Slug = sub.Slug,
                    StatusName = sub.Status?.Name
                }).ToList()
            
        }).ToList();   

        return Ok(dto);
    }

    [HttpGet("listing")]
    public async Task<ActionResult<IEnumerable<ListingCategoryDTO>>> GetCategoriesListing()
    {
        var dto = await _context.Categories
        .Include(x => x.SubCategories)
            .ThenInclude(xx => xx.Status)
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
                    StatusName = sub.Status.Name
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

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "Categories").FirstOrDefaultAsync();
        if (status == null) { return BadRequest("Status is not found"); }
        //var sta = await _context.Statuses.FirstOrDefaultAsync(c => c.Id == dto.StatusId);
        //if (sta == null) { return BadRequest("Status is not found"); }
        //if (sta.Name != "Active") { return BadRequest("wrong status name or type"); }
        //if (sta.Type != "Categories") { return BadRequest("wrong status type"); }

        //subcategories
        //var newSubCats = new List<SubCategoryModel>();
        //if (dto.SubCategoryIds.Any())
        //{
        //    newSubCats = await _context.SubCategories
        //        .Where(sx => dto.SubCategoryIds.Contains(sx.Id))
        //        .ToListAsync();

        //    var missingIds = dto.SubCategoryIds.Except(newSubCats.Select(sx => sx.Id)).ToList();
        //    if (missingIds.Any())
        //    {
        //        return BadRequest($"Các SubCategory sau không tồn tại: {string.Join(", ", missingIds)}");
        //    }
        //}


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
            StatusId = status.Id,
            //SubCategories = newSubCats
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        //foreach (var sub in newSubCats)
        //{
        //    sub.CategoryId = category.Id;
        //}
        //await _context.SaveChangesAsync();

        var catdto = new CardCategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            ImageURL = category.Image?.URL
        };

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, catdto);
    }

    [HttpPut("{id}")]
    public async Task <ActionResult<UpdateCategoryDTO>> UpdateCategory(int id, [FromForm] UpdateCategoryDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cat = await _context.Categories
            .Include(c => c.Image)
            .Include(c => c.SubCategories)
                .ThenInclude(cc => cc.Status)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (cat == null) return NotFound("Danh mục không tồn tại.");

        var sta = await _context.Statuses.FirstOrDefaultAsync(st => st.Id == dto.StatusId);
        if(sta == null) { return BadRequest("Status ko ton tai"); }
        if(sta.Type != "Categories") { return BadRequest("Status ko danh cho categoy!"); }

        //subcategories
        var newSubCats = new List<SubCategoryModel>();
        if (dto.SubCategoryIds.Any())
        {
            newSubCats = await _context.SubCategories
                .Where(sx => dto.SubCategoryIds.Contains(sx.Id))
                .ToListAsync();

            var missingIds = dto.SubCategoryIds.Except(newSubCats.Select(sx => sx.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest($"Các SubCategory sau không tồn tại: {string.Join(", ", missingIds)}");
            }
        }

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

            if (cat.Image != null)
            {
                await _imageUploadService.DeleteAsync(cat.Image.PublicId);
                _context.Images.Remove(cat.Image);
            }

            cat.Image = newImg;
        }

        cat.Name = dto.Name;
        cat.Description = dto.Description;
        cat.StatusId = dto.StatusId;
        cat.UpdatedAt = DateTime.UtcNow;

        foreach(var sub in newSubCats)
        {
            sub.CategoryId = cat.Id;
        }

        await _context.SaveChangesAsync();
        var resultdto = new CategoryDTO
        {
            Id = cat.Id,
            Name = cat.Name,
            Description = cat.Description,
            StatusName = cat.Status.Name,
            ImageURL = cat.Image.URL,
            CreatedAt = cat.CreatedAt,
            UpdatedAt = cat.UpdatedAt,
            SubCategories = cat.SubCategories
                .OrderBy(ss => ss.CreatedAt)
                .Select(ss => new ListingSubCategoryDTO
                {
                    Id = ss.Id,
                    Name = ss.Name,
                    Slug = ss.Slug,
                    StatusName = ss.Status.Name
                }).ToList()
        };

        return Ok(resultdto);

    }
}
