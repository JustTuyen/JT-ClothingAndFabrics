using Amazon.S3.Model;
using clothing_be.Data;
using clothing_be.DTO.production.tag;
using clothing_be.DTO.production.vatians;
using clothing_be.Models.productions.Varied;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttributeValuesModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public AttributeValuesModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: ATTRIBUTEVALUESMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AttributeValuesModel>>> GetAll()
    {
        var attrvalues = await _context.AttributeValues
            .Include(tr => tr.Attribute)
            .OrderBy(tr => tr.CreatedAt)
            .ToListAsync();
        var dto = attrvalues.Select(attrvalue => new AttributeValuesDTO
        {
            Id = attrvalue.Id,
            Value = attrvalue.Value,
            AttriName = attrvalue.Attribute?.Name,
            AttriType = attrvalue.Attribute?.Type
        });

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<AttributeValuesDTO>> GetById(int id)
    {
        var attrvalue = await _context.AttributeValues
            .Include(y => y.Attribute)
            .FirstOrDefaultAsync(i => i.Id == id);

        if(attrvalue == null) { return NotFound(); }

        var dto = new AttributeValuesDTO
        {
            Id = attrvalue.Id,
            Value = attrvalue.Value,
            AttriName = attrvalue.Attribute?.Name,
            AttriType = attrvalue.Attribute?.Type
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<AttributeValuesModel>> CreateAttributeValue([FromBody] CreateAttributeValuesDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var attr = await _context.Attributes
            .FirstOrDefaultAsync(d => d.Id == dto.AttributeId);

        if (attr == null) { return BadRequest("Attribute cant find"); }

        var value = new AttributeValuesModel
        {
            Value = dto.Value,
            AttributeId = dto.AttributeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _context.AttributeValues.Add(value);
        await _context.SaveChangesAsync();

        var resultDto = new AttributeValuesDTO
        {
            Id = value.Id,
            Value = value.Value,
            AttriName = value.Attribute?.Name,
            AttriType = value.Attribute?.Type

        };
        return CreatedAtAction(nameof(GetById), new { id = value.Id }, resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AttributeModel>> DeleteAttribute(int id)
    {
        var var = await _context.Attributes
            .FirstOrDefaultAsync(va => va.Id == id);

        if (var == null) { return BadRequest("Attributes with this id cant found"); }
        _context.Attributes.Remove(var);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{attributeId}/Values/{valueId}")]
    public async Task<IActionResult> DeleteAttributeValue(int attributeId, int valueId)
    {
        var val = await _context.AttributeValues.FirstOrDefaultAsync(v => v.Id == valueId && v.AttributeId == attributeId);
        if (val == null) return NotFound();

        _context.AttributeValues.Remove(val);
        await _context.SaveChangesAsync();

        return NoContent();
    }

}
