using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileAdvisor.API.DTOs.Consultations;
using MobileAdvisor.BLL.DTOs.Consultations;
using MobileAdvisor.BLL.DTOs.Messages;
using MobileAdvisor.BLL.Services.Interfaces;

namespace MobileAdvisor.API.Controllers;

[Route("api/[controller]")]
[Authorize]
public class ConsultationsController : BaseApiController
{
    private readonly IConsultationService _consultationService;

    public ConsultationsController(IConsultationService consultationService)
    {
        _consultationService = consultationService;
    }

    [HttpPost("new-phone")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateNewPhoneConsultation([FromBody] CreateNewPhoneConsultationDto dto)
    {
        try
        {
            var result = await _consultationService.CreateNewPhoneConsultationAsync(CurrentUserId, dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("used-phone")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateUsedPhoneConsultation(
        [FromForm] CreateUsedPhoneConsultationDto dto,
        [FromForm] List<IFormFile>? images)
    {
        try
        {
            // Validate images count (1 to 5) and format (JPEG, PNG, WebP) as per FR-09 & NFR-04
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileList = new List<(string FileName, Stream Stream, string ContentType)>();

            if (images != null)
            {
                if (images.Count > 5)
                {
                    return BadRequest(new { message = "Maximum 5 images are allowed." });
                }

                foreach (var file in images)
                {
                    var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(ext))
                    {
                        return BadRequest(new { message = $"Image format {ext} is not supported. Use JPEG, PNG, or WebP." });
                    }
                    if (file.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest(new { message = $"Image {file.FileName} exceeds 5 MB limit." });
                    }

                    fileList.Add((file.FileName, file.OpenReadStream(), file.ContentType));
                }
            }

            var result = await _consultationService.CreateUsedPhoneConsultationAsync(CurrentUserId, dto, fileList);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _consultationService.GetByIdAsync(id, CurrentUserId, CurrentUserRole);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyConsultations()
    {
        var result = await _consultationService.GetMyConsultationsAsync(CurrentUserId, CurrentUserRole);
        return Ok(result);
    }

    [HttpPost("{id}/review-order")]
    [Authorize(Roles = "Expert")]
    public async Task<IActionResult> ExpertReviewOrder(int id, [FromBody] ExpertReviewActionDto action)
    {
        try
        {
            await _consultationService.ExpertReviewOrderAsync(id, CurrentUserId, action.Accept, action.AgreedPrice);
            return Ok(new { message = "Order review recorded successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/approve-price")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CustomerApprovePrice(int id, [FromBody] CustomerApprovePriceDto action)
    {
        try
        {
            await _consultationService.CustomerApprovePriceAsync(id, CurrentUserId, action.Accept);
            return Ok(new { message = "Price response recorded." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/pay")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> ProcessPayment(int id, [FromBody] ProcessPaymentDto dto)
    {
        try
        {
            await _consultationService.ProcessTestPaymentAsync(id, CurrentUserId, dto);
            return Ok(new { message = dto.IsSuccessful ? "Payment successful. Consultation is ready!" : "Payment failed." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/recommendation")]
    [Authorize(Roles = "Expert")]
    public async Task<IActionResult> AddRecommendation(int id, [FromBody] CreateRecommendationDto dto)
    {
        try
        {
            await _consultationService.AddRecommendationAsync(id, CurrentUserId, dto);
            return Ok(new { message = "Recommendation added." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/complete")]
    [Authorize(Roles = "Expert")]
    public async Task<IActionResult> CompleteConsultation(int id)
    {
        try
        {
            await _consultationService.CompleteConsultationAsync(id, CurrentUserId);
            return Ok(new { message = "Consultation completed." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/review")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddReview(int id, [FromBody] CreateReviewDto dto)
    {
        try
        {
            await _consultationService.AddReviewAsync(id, CurrentUserId, dto);
            return Ok(new { message = "Review submitted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/messages")]
    public async Task<IActionResult> SendMessage(int id, [FromBody] SendMessageDto dto)
    {
        try
        {
            var msg = await _consultationService.SendMessageAsync(id, CurrentUserId, dto.Content);
            return Ok(msg);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> GetMessages(int id)
    {
        try
        {
            var messages = await _consultationService.GetMessagesAsync(id, CurrentUserId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
