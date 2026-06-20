using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.BLL.Services.prep.DetectionService;
using NextLIMS.BLL.Services.prep.EnumerationService;

namespace NexLIMS.API.Controllers.Sample
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleTestController : ControllerBase
    {
        private readonly IEnumerationService _enumerationService;
        private readonly IDetectionService _detectionService;

        public SampleTestController(IEnumerationService enumerationService, IDetectionService detectionService)
        {
            _enumerationService = enumerationService;
            _detectionService = detectionService;
        }

        [Authorize]
        [HttpPost("{sampleTestId}/enumeration-prep")]
        [CheckPermission("Save_Enumeration_Prep")]
        public async Task<IActionResult> SaveEnumerationPrep([FromRoute] int sampleTestId, [FromBody] EnumerationPrepDto dto)
        {
            if (dto == null) return BadRequest("Payload cannot be empty.");
            int preparationSaved = await _enumerationService.SaveEnumerationPrepAsync(sampleTestId, dto);
            return Ok(new { message = preparationSaved > 0 ? "Data saved successfully." : "Failed to save data Try Again." });
        }

        [Authorize]
        [HttpPost("{sampleTestId}/detection-prep")]
        [CheckPermission("Save_Detection_Prep")]
        public async Task<IActionResult> SaveDetectionPrep([FromRoute] int sampleTestId, [FromBody] DetectionPrepDto dto)
        {
            if (dto == null) return BadRequest("Payload cannot be empty.");
            int preparationSaved = await _detectionService.SaveDetectionPrepAsync(sampleTestId, dto);
            return Ok(new { message = preparationSaved > 0 ? "Data saved successfully." : "Failed to save data Try Again." });
        }

        [Authorize]
        [HttpPost("{sampleTestId}/molecular-prep")]
        [CheckPermission("Save_Molecular_Prep")]
        public async Task<IActionResult> SaveMolecularPrep([FromRoute] int sampleTestId, [FromBody] DetectionPrepDto dto)
        {
            if (dto == null) return BadRequest("Payload cannot be empty.");
            int preparationSaved = await _detectionService.SaveDetectionPrepAsync(sampleTestId, dto);
            return Ok(new { message = preparationSaved > 0 ? "Data saved successfully." : "Failed to save data Try Again." });
        }
    }
}
