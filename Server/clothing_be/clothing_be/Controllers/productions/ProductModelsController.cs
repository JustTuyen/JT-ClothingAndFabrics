
using Amazon.S3.Model;
using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production;
using clothing_be.DTO.production.tag;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.Others;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Tagging;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]

public class ProductModelsController : ControllerBase
{
    private readonly IImageUploadService _imageUploadService;
    private readonly MyDbContextApplication _context;

    public ProductModelsController(MyDbContextApplication context, IImageUploadService imageUploadService)
    {
        _context = context;
        _imageUploadService = imageUploadService;
    }

    // GET: PRODUCTMODELS
    [HttpGet]
    public async Task <ActionResult<IEnumerable<ProductDTO>>> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Status)
            .Include(p => p.Discount)
            .Include(p => p.ImageGalleries)
                .ThenInclude(p => p.Image)
            .Include(p => p.SubCategory)
            .Include(p=> p.ProductTags)
                .ThenInclude(p=>p.Tag)
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

                }).ToList(),
            Tags = product.ProductTags
                .Select(t => new MiniProductTagDTO
                {
                    Id = t.Id,
                    TagName = t.Tag.Name
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
                .ThenInclude(p => p.Image)
            .Include(p => p.SubCategory)
            .Include(p => p.ProductTags)
                .ThenInclude(p => p.Tag)
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

                }).ToList(),
            Tags = product.ProductTags
                .Select(t => new MiniProductTagDTO
                {
                    Id = t.Id,
                    TagName = t.Tag.Name
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
        if (sta.Name != "Active") { return BadRequest("wrong status name or type"); }
        if (sta.Type != "Products") { return BadRequest("wrong status type"); }

        var subcat = await _context.SubCategories.Include(st=> st.Status).FirstOrDefaultAsync(st => st.Id == dto.SubCategoryId);
        if (subcat == null) { return BadRequest("This SubCategories is missing or doesnt exist!"); }
        if (subcat.Status?.Name != "Active") { return BadRequest("This subcategories is closed"); }
        if (subcat.Status.Type != "SubCategories") { return BadRequest("This subcategories is not for product"); }



        //image 
        var imageGalleries = new List<ImageGalleryModel>();
        if (dto.Images != null && dto.Images.Any())
        {
            int displayOrder = 0;
            foreach(var img in dto.Images)
            {
                var uploadResult = await _imageUploadService.UploadAsync(img);
                var image = new ImageModel
                {
                    URL = uploadResult.Url,
                    PublicId = uploadResult.Key,
                    Width = uploadResult.Width,
                    Height = uploadResult.Height,
                    Type = img.ContentType,
                    CreatedAt = DateTime.UtcNow,
                    IsThumbnail = displayOrder == 0,
                };

                imageGalleries.Add(new ImageGalleryModel
                {
                    Image = image,
                    DisplayOrder = displayOrder++
                });
            }
        }


        var productTags = new List<ProductTagModel>();
        if (dto.TagIds != null && dto.TagIds.Any())
        {
            foreach (var tagId in dto.TagIds)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == tagId);
                if (tag == null) { return BadRequest($"Invalid TagID: {tagId}"); }
                productTags.Add(new ProductTagModel
                {
                    TagId = tagId,
                });

            }
        }


        var product = new ProductModel
        {
            Name = dto.Name,
            Description = dto.Description,
            BasePrice = dto.BasePrice,
            SubCategoryId = dto.SubCategoryId,
            StatusId = dto.StatusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ImageGalleries = imageGalleries,
            ProductTags = productTags
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

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateProductDTO>> UpdateProduct(int id, [FromForm] UpdateProductDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var sta = await _context.Statuses.FirstOrDefaultAsync(st => st.Id == dto.StatusId);
        if (sta == null) { return BadRequest("This status is missing or doesnt exist!"); }
        if (sta.Type != "Products") { return BadRequest("wrong status type"); }

        var subcat = await _context.SubCategories.Include(st => st.Status).FirstOrDefaultAsync(st => st.Id == dto.SubCategoryId);
        if (subcat == null) { return BadRequest("This SubCategories is missing or doesnt exist!"); }
        if (subcat.Status == null || subcat.Status.Type != "SubCategories") { return BadRequest("This subcategories is not for product"); }

        var pro = await _context.Products
            .Include(p => p.ProductTags)
            .Include(p => p.ImageGalleries)
                .ThenInclude(ig => ig.Image)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if(pro == null) { return BadRequest("This Product is missing or doesnt exist!"); }

        if (dto.Images != null && dto.Images.Any())
        {
            foreach (var oldGallery in pro.ImageGalleries)
            {
                if (oldGallery.Image != null)
                    await _imageUploadService.DeleteAsync(oldGallery.Image.PublicId);
            }

            _context.ImageGalleries.RemoveRange(pro.ImageGalleries);
            _context.Images.RemoveRange(pro.ImageGalleries.Select(g => g.Image).Where(img => img != null)!);

            var newGalleries = new List<ImageGalleryModel>();
            int displayOrder = 0;
            foreach (var img in dto.Images)
            {
                var uploadResult = await _imageUploadService.UploadAsync(img);
                var image = new ImageModel
                {
                    URL = uploadResult.Url,
                    PublicId = uploadResult.Key,
                    Width = uploadResult.Width,
                    Height = uploadResult.Height,
                    Type = img.ContentType,
                    CreatedAt = DateTime.UtcNow,
                    IsThumbnail = displayOrder == 0,
                };

                newGalleries.Add(new ImageGalleryModel
                {
                    Image = image,
                    DisplayOrder = displayOrder++
                });
            }

            pro.ImageGalleries = newGalleries;
        }

        if (dto.TagIds != null && dto.TagIds.Any())
        {
            var existingTags = await _context.Tags
                .Where(t => dto.TagIds.Contains(t.Id))
                .ToListAsync();

            var missingTagIds = dto.TagIds.Except(existingTags.Select(t => t.Id)).ToList();
            if (missingTagIds.Any())
                return BadRequest($"Invalid TagIDs: {string.Join(", ", missingTagIds)}");

            pro.ProductTags.Clear();
            pro.ProductTags = existingTags.Select(tag => new ProductTagModel { TagId = tag.Id }).ToList();
        }

        pro.Name = dto.Name;
        pro.Description = dto.Description;
        pro.BasePrice = dto.BasePrice;
        pro.StatusId = dto.StatusId;
        pro.SubCategoryId = dto.SubCategoryId;
        pro.UpdatedAt = DateTime.UtcNow;
        

        _context.Products.Update(pro);
        await _context.SaveChangesAsync();

        var resultdto = new UpdateProductDTO
        {
            Name = pro.Name,
            Description = pro.Description,
            BasePrice = pro.BasePrice,
            StatusId = pro.StatusId,
            SubCategoryId = pro.SubCategoryId,
        };

        return Ok(resultdto);
    }
}
