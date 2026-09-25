
using Amazon.S3.Model;
using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production;
using clothing_be.DTO.production.tag;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.Others;
using clothing_be.Models.productions;
using clothing_be.Models.productions.Tagging;
using clothing_be.Models.productions.Varied;
using clothing_be.Services.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.ColorProfiles;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
    public async Task<ActionResult<IEnumerable<MenuProductDTO>>> GetAll()
    {
        var products = await _context.Products
            .Include(p => p.Status)
            .Include(p => p.Discount)
                .ThenInclude(p => p.Status)
            .Include(p => p.ImageGalleries)
                .ThenInclude(p => p.Image)
            
            .OrderBy(p => p.CreatedAt)
            .ToListAsync();

        var dto = products.Select(product => new MenuProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            BasePrice = product.BasePrice,
            DiscountPercentage =  product.Discount != null 
                && product.Discount.Status != null
                && product.Discount.Status.Name == "Active"
                ? product.Discount.Percentage : null,
            StatusName = product.Status?.Name,
            Slug = product.Slug,
            ImageURL = product.ImageGalleries
                .Where(ig => ig.DisplayOrder == 0)
                .Select(ig => ig.Image.URL)
                .FirstOrDefault()

        }).ToList();

        return Ok(dto);


    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDTO>> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Variations)
                .ThenInclude(pp => pp.Status)
            .Include(p => p.Variations)
                .ThenInclude(pp => pp.VariantAttributeValues)
                    .ThenInclude(pp => pp.AttributeValue)
                        .ThenInclude(pp => pp.Attribute)
            .Include(p => p.Status)
            .Include(p => p.Discount)
                .ThenInclude(p => p.Status)
            .Include(p => p.ImageGalleries)
                .ThenInclude(p => p.Image)
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null) { return NotFound(); }

        product.ViewCount++;
        await _context.SaveChangesAsync();

        var dto = new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Slug = product.Slug,
            Description = product.Description,
            BasePrice = product.BasePrice,
            DiscountPercentage = product.Discount != null
                && product.Discount.Status != null
                && product.Discount.Status.Name == "Active"
                ? product.Discount.Percentage : null,
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
                    StockQuantity = pv.StockQuantity,
                    AddPrice = pv.AddPrice,
                    StatusName = pv.Status?.Name,
                    VariantAttributes = pv.VariantAttributeValues
                        .Select(vav => new VariantAttributeValuesDTO
                        {
                            Id = vav.Id,
                            AttributeTypes = vav.AttributeValue.Attribute != null ? vav.AttributeValue.Attribute.Type : null,
                            AttributeValues = vav.AttributeValue != null ? vav.AttributeValue.Value : null

                        }).ToList()

                }).ToList(),
           
        };

        return Ok(dto);

    }

    [HttpPost]
    public async Task<ActionResult<ProductModel>> CreateProduct([FromForm] CreateProductDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "Products").FirstOrDefaultAsync();
        if (status == null) { return BadRequest("Status is not found"); }

        var subcat = await _context.SubCategories.Include(st => st.Status).FirstOrDefaultAsync(st => st.Id == dto.SubCategoryId);
        if (subcat == null) { return BadRequest("This SubCategories is missing or doesnt exist!"); }
        if (subcat.Status?.Name != "Active") { return BadRequest("This subcategories is closed"); }
        if (subcat.Status.Type != "SubCategories") { return BadRequest("This subcategories is not for product"); }



        //image 
        var imageGalleries = new List<ImageGalleryModel>();
        if (dto.Images != null && dto.Images.Any())
        {
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
            StatusId = status.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ImageGalleries = imageGalleries,
            ProductTags = productTags,
            Slug = dto.Slug
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
            //.Include(p =>  p.Variations)
            .Include(p => p.ProductTags)
            .Include(p => p.ImageGalleries)
                .ThenInclude(ig => ig.Image)
            .FirstOrDefaultAsync(pr => pr.Id == id);

        if (pro == null) { return BadRequest("This Product is missing or doesnt exist!"); }

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

        //if (dto.VariationsId != null)
        //{
        //    var existingVariants = await _context.Variations
        //        .Where(v => dto.VariationsId.Contains(v.Id))
        //        .ToListAsync();

        //    var missingVariantIds = dto.VariationsId.Except(existingVariants.Select(t => t.Id)).ToList();
        //    if (missingVariantIds.Any())
        //        return BadRequest($"Invalid VariantID: {string.Join(", ", missingVariantIds)}");

        //    pro.Variations.Clear();
        //    pro.Variations = existingVariants.Select(Variation => new VariationModel { ProductId = pro.Id }).ToList();
        //}

        pro.Name = dto.Name;
        pro.Description = dto.Description;
        pro.BasePrice = dto.BasePrice;
        pro.StatusId = dto.StatusId;
        pro.SubCategoryId = dto.SubCategoryId;
        pro.UpdatedAt = DateTime.UtcNow;
        pro.Slug = dto.Slug;


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

    [HttpGet("/keyword")]
    public async Task<ActionResult<MenuProductDTO>> SearchByKeyWord([FromQuery] string keyword)
    {
        if (string.IsNullOrEmpty(keyword))
        {
            return BadRequest("keyword = null");
        }

        var pros = await _context.Products
            .Where(p => EF.Functions.ILike(p.Name, $"%{keyword}%"))
            .Include(p => p.Status)
            .Include(p => p.Discount)
                .ThenInclude(p => p.Status)
            .Include(p => p.ImageGalleries)
                .ThenInclude(pp => pp.Image)
            .ToListAsync();

        var dto = pros.Select(product => new MenuProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            BasePrice = product.BasePrice,
            DiscountPercentage = product.Discount != null
                && product.Discount.Status != null
                && product.Discount.Status.Name == "Active"
                ? product.Discount.Percentage : null,
            StatusName = product.Status?.Name,
            Slug = product.Slug,
            ImageURL = product.ImageGalleries
                .Where(ig => ig.DisplayOrder == 0)
                .Select(ig => ig.Image.URL)
                .FirstOrDefault()

        }).ToList();

        return Ok(dto);

    }

    [HttpPut("{id}/tags")]
    public async Task<ActionResult<UpdateTagProductDTO>> UpdateTheTag(int id, [FromForm] UpdateTagProductDTO dto)
    {
        var pro = await _context.Products.FirstOrDefaultAsync(o => o.Id == id);
        if (pro == null) { return BadRequest("This Product is missing or doesnt exist!"); }

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

        await _context.SaveChangesAsync();

        return Ok(new { mes = "tag updated!" });
    }

    [HttpGet("filter")]
    public async Task <ActionResult<IEnumerable<MenuProductDTO>>> FilterProducts([FromQuery] ProductFilterDTO filter)
    {
        var query = _context.Products.AsQueryable();

        query = query.Where(p => p.Status!.Name == "Active");

        if (filter.MinPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice >= filter.MinPrice);
        }

        if (filter.MaxPrice.HasValue)
        {
            query = query.Where(p => p.BasePrice <= filter.MaxPrice);
        }

        if (filter.CreatedFrom.HasValue)
        {
            query = query.Where(p => p.CreatedAt >= filter.CreatedFrom);
        }

        if (filter.CreatedTo.HasValue)
        {
            query = query.Where(p => p.CreatedAt <= filter.CreatedTo);
        }

        if (filter.MinViewCount.HasValue)
        {
            query = query.Where(p => p.ViewCount >= filter.MinViewCount);
        }

        if (filter.HasDiscount == true)
        {
            var now = DateTime.UtcNow;
            query = query.Where(p => p.Discount != null
                && p.Discount.StartAt <= now
                && p.Discount.ExpireAt >= now
            );
        }

        query = filter.SortBy?.ToLower() switch
        {
            "viewcount" => filter.SortDescending
                ? query.OrderByDescending(p => p.ViewCount)
                : query.OrderBy(p => p.ViewCount),

            "price" => filter.SortDescending
                ? query.OrderByDescending(p => p.BasePrice)
                : query.OrderBy(p => p.BasePrice),

            "discount" => filter.SortDescending
                ? query.OrderByDescending(p => p.Discount != null ? p.Discount.Percentage : 0)
                : query.OrderBy(p => p.Discount != null ? p.Discount.Percentage : 0),

            _ => filter.SortDescending
                ? query.OrderByDescending(p => p.CreatedAt)
                : query.OrderBy(p => p.CreatedAt)
        };

        var totalCount = await query.CountAsync();
        var pros = await query.Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(p => new MenuProductDTO
            {
                Id = p.Id,
                BasePrice = p.BasePrice,
                Name = p.Name,
                StatusName = p.Status != null ? p.Status.Name : null,
                Slug = p.Slug,
                DiscountPercentage = p.Discount != null ? p.Discount.Percentage : null,
                ImageURL = p.ImageGalleries
                .Where(ig => ig.DisplayOrder == 0)
                .Select(ig => ig.Image!.URL)
                .FirstOrDefault()

            }).ToListAsync();

        Response.Headers.Append("X-Total-Count", totalCount.ToString());
        return Ok(pros);
    }

    


    [HttpGet("detail/{id}")]
    public async Task<ActionResult<ProductModallDTO>> GetProductDetail(int id)
    {
        var pro = await _context.Products
            .Include(p => p.Status)
            .Include(p => p.Discount)
                .ThenInclude(p => p.Status)
            .Include(p => p.SubCategory)
            .Include(p => p.Variations)
                .ThenInclude(p => p.VariantAttributeValues)
                    .ThenInclude(p => p.AttributeValue)
                        .ThenInclude(p => p.Attribute)
            .Include(p => p.ImageGalleries)
                .ThenInclude(p => p.Image)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pro == null) return BadRequest("this product is null");

        var dto = new ProductModallDTO
        {
            Id = pro.Id,
            Name = pro.Name,
            BasePrice = pro.BasePrice,
            DiscountPercentage = pro.Discount != null
                && pro.Discount.Status != null
                && pro.Discount.Status.Name == "Active"
                ? pro.Discount.Percentage : null,
            StatusName = pro.Status != null ? pro.Status.Name : null,
            SubCategoryName = pro.SubCategory != null ? pro.SubCategory.Name : null,
            Variations = pro.Variations
                .OrderBy(va => va.CreatedAt)
                .Select(va => new ListingVariationDTO
                {
                    Id = va.Id,
                    AddPrice = va.AddPrice,
                    StockQuantity = va.StockQuantity,
                    StatusName = va.Status != null ? va.Status.Name : null,
                    VariantAttributes = va.VariantAttributeValues
                        .Select(vav => new VariantAttributeValuesDTO
                        {
                            Id = vav.Id,
                            AttributeTypes = vav.AttributeValue.Attribute.Name,
                            AttributeValues = vav.AttributeValue != null ? vav.AttributeValue.Value : null
                        }).ToList()
                }).ToList(),
            ImageGalleries = pro.ImageGalleries
                .OrderBy(ig => ig.DisplayOrder)
                .Select(ig => new MiniImageGalleryDTO
                {
                    Id = ig.Id,
                    DisplayOrder = ig.DisplayOrder,
                    ImageURL = ig.Image != null ? ig.Image.URL : null
                }).ToList()
        };

        return Ok(dto);
    }
    


}
