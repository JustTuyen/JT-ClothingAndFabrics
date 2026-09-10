
using clothing_be.Data;
using clothing_be.DTO.core;
using clothing_be.DTO.production.category;
using clothing_be.Models.Others;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//FOR API 
[ApiController]
[Route("api/[controller]")]
public class StatusModelsController : ControllerBase
{
    private readonly MyDbContextApplication _context;

    public StatusModelsController(MyDbContextApplication context)
    {
        _context = context;
    }

    // GET: STATUSMODELS
    [HttpGet]
    public async Task <ActionResult<IEnumerable<StatusModel>>> GetAllStatus()
    {
        var statuses = await _context.Statuses
            .OrderBy(s => s.Type)
            .ToListAsync();
        var dto = statuses.Select(sta => new StatusDTO
        {
            Id = sta.Id,
            Type = sta.Type,
            Name = sta.Name,
        });

        return Ok(dto);
    }

    [HttpGet("{id}")]
    public async Task <ActionResult<StatusDTO>> GetStatus(int id)
    {
        var dto = await _context.Statuses
            .FirstOrDefaultAsync(s => s.Id == id);


        if(dto == null) return NotFound();

        return Ok(dto); 
    }

    [HttpPost]
    public async Task <ActionResult<StatusModel>> CreateStatus([FromBody] CreateStatusDTO dto)
    {
        if (dto == null) return BadRequest("Dto or request data is missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var exist = await _context.Statuses
            .AnyAsync( s => s.Name == dto.Name && s.Type == dto.Type);
        if (exist) return BadRequest($"Status '{dto.Name}' với Type '{dto.Type}' đã tồn tại.");
        var status = new StatusModel
        {
            Name = dto.Name,
            Type = dto.Type,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _context.Statuses.Add(status);
        await _context.SaveChangesAsync();

        var stadto = new StatusDTO
        {
            Id = status.Id,
            Name = status.Name,
            Type = status.Type,
        };

        return CreatedAtAction(nameof(GetStatus), new { id = status.Id }, stadto);
    }

    [HttpPut("{id}")]
    public async Task <ActionResult<StatusModel>> PutStatusById (int id, [FromBody] UpdatedStatusDTO dto)
    {
        if (dto == null) return BadRequest("Dto is null or missing");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var cat = await _context.Statuses
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cat == null) return NotFound();
        cat.Name = dto.Name;
        cat.Type = dto.Type;
        cat.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedSta = await _context.Statuses
            .FirstOrDefaultAsync(c => c.Id == id);

        var updatedto = new StatusDTO
        {
            Id = id,
            Name = updatedSta.Name,
            Type = updatedSta.Type,
        };

        return Ok(updatedto);
    }
}
