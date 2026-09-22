
using clothing_be.Data;
using clothing_be.DTO.production.tag;
using clothing_be.Models.Others;
using clothing_be.Models.productions.Tagging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class TagModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public TagModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: TAGMODELS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StatusModel>>> GetAllTag()
    {
        var tags = await _context.Tags
            .Include(t => t.Status)
            .OrderBy(t => t.CreatedAt)
            .ToListAsync();

        var dto = tags.Select(tag => new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            Statusname = tag.Status.Name
        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> GetById(int id)
    {
        var tag = await _context.Tags
            .Include(t => t.Status)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (tag == null) { return NotFound(); }
        var dto = new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            Statusname = tag.Status?.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TagDTO>> CreateTag([FromBody] CreateTagDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var status = await _context.Statuses.Where(st => st.Name == "Active" && st.Type == "Tags").FirstOrDefaultAsync();
        if (status == null) { return BadRequest("Status is not found"); }

        var tag = new TagModel
        {
            Name = dto.Name,
            StatusId = status.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        var resultDto = new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            Statusname = tag.Status?.Name

        };
        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, resultDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<StatusModel>> DeleteStatus(int id)
    {
        var status = await _context.Statuses.FirstOrDefaultAsync(x => x.Id == id);

        if (status == null) { return BadRequest("Cant find a Status with this id"); }
        _context.Statuses.Remove(status);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UpdateTagDTO>> UpdateTag(int id, [FromBody] UpdateTagDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tag = await _context.Tags.Include(tg => tg.Status).FirstOrDefaultAsync(tg => tg.Id == id);
        if (tag == null) { return BadRequest("Cant find tag with this is id"); }

        var sta = await _context.Statuses.FirstOrDefaultAsync(x => x.Id == dto.StatusId);
        if (sta == null) { return BadRequest("Cant find status with this is id"); }
        if (sta.Type != "Tags") { return BadRequest("This status is not for tag"); }

        tag.Name = dto.Name;
        tag.StatusId = dto.StatusId;
        tag.UpdatedAt = DateTime.UtcNow;

        var resultdto = new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            Statusname = tag.Status.Name,

        };

        await _context.SaveChangesAsync();
        return Ok(resultdto);
    }

    
}
