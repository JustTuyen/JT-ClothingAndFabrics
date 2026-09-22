
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
            .OrderBy(x => x.CreatedAt)
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

    [HttpGet("${type}")]
    public async Task<ActionResult<AttributeDTO>> GetByType(string type)
    {
        var attrs = await _context.Attributes
            .Include(x => x.AttributeValues)
            .OrderBy(x => x.CreatedAt)
            .Where(x => x.Type == type)
            .ToListAsync();

        var dto = attrs.Select(attr => new AttributeDTO
        {
            Id = attr.Id,
            Name = attr.Name,
            Type = attr.Type,
            AttributeValues = attr.AttributeValues
                .OrderBy(a => a.CreatedAt)
                .Select(a => new MiniAttributeValuesDTO
                {
                    Id = a.Id,
                    Value = a.Value
                }).ToList()
        }).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<AttributeDTO>> CreateAttribute([FromForm] CreateAttributeDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        //attribute value
        //var newValues = new List<AttributeValuesModel>();
        //if (dto.AttributeValueIds.Any())
        //{
        //    newValues = await _context.AttributeValues
        //        .Where(s => dto.AttributeValueIds.Contains(s.Id))
        //        .ToListAsync();

        //    var missingIds = dto.AttributeValueIds.Except(newValues.Select(sx => sx.Id)).ToList();
        //    if (missingIds.Any())
        //    {
        //        return BadRequest($"Các Attribute Value sau không tồn tại: {string.Join(", ", missingIds)}");
        //    }
        //}

        var attr = new AttributeModel
        {
            Name = dto.Name,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            //AttributeValues = newValues
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

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateAttributeDTO>> UpdateAttr(int id, [FromForm] UpdateAttributeDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var attr = await _context.Attributes.FirstOrDefaultAsync(v => v.Id == id);
        if (attr == null) { return BadRequest("Cant find Attribte with this ID"); }

        var newValues = new List<AttributeValuesModel>();
        if (dto.AttributeValueIds.Any())
        {
            newValues = await _context.AttributeValues
                .Where(s => dto.AttributeValueIds.Contains(s.Id))
                .ToListAsync();

            var missingIds = dto.AttributeValueIds.Except(newValues.Select(sx => sx.Id)).ToList();
            if (missingIds.Any())
            {
                return BadRequest($"Các Attribute Value sau không tồn tại: {string.Join(", ", missingIds)}");
            }
        }

        attr.Name = dto.Name;
        attr.Type = dto.Type;
        attr.UpdatedAt = DateTime.UtcNow;
        
        foreach(var va in newValues)
        {
            va.AttributeId = attr.Id;
        }

        await _context.SaveChangesAsync();

        var resultdto = new AttributeDTO
        {
            Id = attr.Id,
            Name = attr.Name,
            Type = attr.Type,
            AttributeValues = attr.AttributeValues
                .OrderBy(s => s.CreatedAt)
                .Select(s => new MiniAttributeValuesDTO
                {
                    Id = s.Id,
                    Value = s.Value
                }).ToList()
        };

        return Ok(resultdto);

    }

    [HttpDelete("{id}")]
    public async Task <ActionResult<AttributeModel>> DeleteVariation(int id)
    {
        var var = await _context.Attributes
            .FirstOrDefaultAsync(va => va.Id == id);

        if(var == null) { return BadRequest("Attributes with this id cant found"); }
        _context.Attributes.Remove(var);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
