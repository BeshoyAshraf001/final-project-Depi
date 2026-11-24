using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAdvisor.BLL.DTOs.Phones;
using MobileAdvisor.BLL.Services.Interfaces;

namespace MobileAdvisor.API.Controllers;

[Route("api/[controller]")]
public class PhonesController : BaseApiController
{
    private readonly IPhoneService _phoneService;

    public PhonesController(IPhoneService phoneService)
    {
        _phoneService = phoneService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var phones = await _phoneService.GetAllPhonesAsync();
        return Ok(phones);
    }

    [HttpGet("recommend")]
    public async Task<IActionResult> Recommend([FromQuery] decimal budget, [FromQuery] string? keyword)
    {
        var phones = await _phoneService.RecommendPhonesAsync(budget, keyword);
        return Ok(phones);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdatePhoneDto dto)
    {
        var created = await _phoneService.CreatePhoneAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrUpdatePhoneDto dto)
    {
        try
        {
            await _phoneService.UpdatePhoneAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _phoneService.DeletePhoneAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
