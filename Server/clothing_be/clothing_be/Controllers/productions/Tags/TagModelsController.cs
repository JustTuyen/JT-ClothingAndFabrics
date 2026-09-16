
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
            Status = tag.Status.Name
        }).ToList();

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDTO>> GetById(int id)
    {
        var tag = await _context.Tags
            .Include(t => t.Status)
            .FirstOrDefaultAsync(i => i.Id == id);

        if(tag == null) { return NotFound(); }

        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDTO>> CreateTag([FromBody] CreateTagDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);


        var status = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Id == dto.StatusId);

        if (status == null)
            return BadRequest("Status không tồn tại.");

        if (status.Type != "Tag" || status.Name != "Active")
            return BadRequest("Status is either not for tag or not active!");

        var tag = new TagModel
        {
            Name = dto.Name,
            StatusId = dto.StatusId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        var resultDto = new TagDTO
        {
            Id = tag.Id,
            Name = tag.Name,
            Status = tag.Status.Name
         
        }; 
        return CreatedAtAction(nameof(GetById), new { id = tag.Id }, resultDto);
    }
}
