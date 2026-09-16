
using Amazon.S3.Model;
using clothing_be.Data;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.productions.Varied;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttributeModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public AttributeModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: ATTRIBUTEMODELS
    [HttpGet]
    public async Task <ActionResult<IEnumerable<AttributeDTO>>> GetAll()
    {
        var attrs = await _context.Attributes
            .Include(a => a.AttributeValues)
            .OrderBy(a => a.CreatedAt)
            .ToListAsync();

        var dto = attrs.Select(attr => new AttributeDTO
        {
            Id = attr.Id,
            Name = attr.Name,
            Type = attr.Type,
            AttributeValues = attr.AttributeValues
                .OrderBy(v => v.CreatedAt)
                .Select(v => new MiniAttributeValuesDTO
                {
                    Id = v.Id,
                    Value = v.Value
                }).ToList()
            
        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<AttributeDTO>> GetById(int id)
    {
        var attr = await _context.Attributes
            .Include(t => t.AttributeValues)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attr == null) { return NotFound(); }

        var dto = new AttributeDTO
        {
            Id = attr.Id,
            Name = attr.Name,
            Type = attr.Type,
            AttributeValues = attr.AttributeValues
                .OrderBy(v => v.CreatedAt)
                .Select(v => new MiniAttributeValuesDTO
                {
                    Id = v.Id,
                    Value = v.Value
                }).ToList()
        };
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<AttributeDTO>> CreateAttribute([FromBody] CreateAttributeDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var attr = new AttributeModel
        {
            Name = dto.Name,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Attributes.Add(attr);
        await _context.SaveChangesAsync();

        var resultdto = new AttributeDTO
        {
            Id = attr.Id,
            Name = attr.Name,
            Type = attr.Type
        };

        return CreatedAtAction(nameof(GetById), new { id = attr.Id }, resultdto);
    }
}
