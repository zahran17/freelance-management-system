using Microsoft.AspNetCore.Mvc;
using CDN.Freelancer.Application;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CDN.Freelancer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FreelancersController : ControllerBase
{
    private readonly FreelancerService _service;
    public FreelancersController(FreelancerService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CDN.Freelancer.Domain.Freelancer>>> GetAll(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("Invalid pagination parameters. PageNumber must be >= 1, PageSize must be between 1 and 50.");
        }

        var result = await _service.GetAllPaginatedAsync(pageNumber, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CDN.Freelancer.Domain.Freelancer>> GetById(int id)
    {
        var freelancer = await _service.GetByIdAsync(id);
        if (freelancer == null) return NotFound();
        return Ok(freelancer);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<CDN.Freelancer.Domain.Freelancer>>> Search(
        [FromQuery] string query,
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Search query is required.");
        }

        if (pageNumber < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("Invalid pagination parameters. PageNumber must be >= 1, PageSize must be between 1 and 50.");
        }

        var result = await _service.SearchPaginatedAsync(query, pageNumber, pageSize);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CDN.Freelancer.Domain.Freelancer>> Create([FromBody] CreateFreelancerDto dto)
    {
        var freelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Username = dto.Username,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsArchived = dto.IsArchived,
            Skillsets = dto.Skillsets.Select(s => new CDN.Freelancer.Domain.Skillset { Name = s.Name }).ToList(),
            Hobbies = dto.Hobbies.Select(h => new CDN.Freelancer.Domain.Hobby { Name = h.Name }).ToList()
        };
        
        await _service.AddAsync(freelancer);
        return CreatedAtAction(nameof(GetById), new { id = freelancer.Id }, freelancer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFreelancerDto dto)
    {
        var freelancer = new CDN.Freelancer.Domain.Freelancer
        {
            Id = id,
            Username = dto.Username,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            IsArchived = dto.IsArchived,
            Skillsets = dto.Skillsets.Select(s => new CDN.Freelancer.Domain.Skillset { Name = s.Name }).ToList(),
            Hobbies = dto.Hobbies.Select(h => new CDN.Freelancer.Domain.Hobby { Name = h.Name }).ToList()
        };
        
        await _service.UpdateAsync(freelancer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/archive")]
    public async Task<IActionResult> Archive(int id)
    {
        await _service.ArchiveAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/unarchive")]
    public async Task<IActionResult> Unarchive(int id)
    {
        await _service.UnarchiveAsync(id);
        return NoContent();
    }
} 