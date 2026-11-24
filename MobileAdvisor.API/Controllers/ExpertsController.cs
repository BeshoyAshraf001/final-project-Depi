using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAdvisor.BLL.DTOs.Experts;
using MobileAdvisor.BLL.Services.Interfaces;

namespace MobileAdvisor.API.Controllers;

[Route("api/[controller]")]
public class ExpertsController : BaseApiController
{
    private readonly IExpertService _expertService;

    public ExpertsController(IExpertService expertService)
    {
        _expertService = expertService;
    }

    [HttpGet]
    public async Task<IActionResult> GetApprovedExperts([FromQuery] int? specialtyId)
    {
        var experts = await _expertService.GetApprovedExpertsAsync(specialtyId);
        return Ok(experts);
    }

    [HttpGet("profile")]
    [Authorize(Roles = "Expert")]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await _expertService.GetProfileByUserIdAsync(CurrentUserId);
        if (profile == null) return NotFound(new { message = "Profile not found." });
        return Ok(profile);
    }

    [HttpPut("profile")]
    [Authorize(Roles = "Expert")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateExpertProfileDto dto)
    {
        try
        {
            var updated = await _expertService.UpdateProfileAsync(CurrentUserId, dto);
            return Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{expertProfileId}/approve")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveExpert(int expertProfileId)
    {
        try
        {
            await _expertService.ApproveExpertAsync(expertProfileId);
            return Ok(new { message = "Expert approved successfully." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
