using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexLIMS.API.Middlewares;
using NextLIMS.BLL.DTO.prep;
using NextLIMS.BLL.DTO.PrepDTOs;
using NextLIMS.BLL.Services.prep.DetectionService;
using NextLIMS.BLL.Services.prep.EnumerationService;
using NextLIMS.BLL.Services.prep.LabAnalysisService;
using NextLIMS.DAL.Data.Models;
using NextLIMS.DAL.RepoDTO.sampleData;
using System.Security.Permissions;

namespace NexLIMS.API.Controllers.Sample
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleTestController : ControllerBase
    {
        private readonly IEnumerationService _enumerationService;
        private readonly IDetectionService _detectionService;
        private readonly ILabAnalysisService _labAnalysisService;

        // The DI container will automatically supply this parameter now!
        public SampleTestController(IEnumerationService enumerationService,
                                    IDetectionService detectionService , 
                                    ILabAnalysisService labAnalysisService)
        {
            _enumerationService = enumerationService;
            _detectionService = detectionService;
            _labAnalysisService = labAnalysisService;
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
        [Authorize]
        [HttpPost("{sampleTestId}/enumeration-prep/result-Submit")]
        [CheckPermission("Save_Enumeration_Prep_Data")]
        public async Task<IActionResult> SaveEnumerationPrepData([FromRoute] int sampleTestId, [FromBody] SaveLabDataRequest request)
        {
            if (request == null) return BadRequest("Request body cannot be null.");

            try
            {
                var reponse = await _labAnalysisService.ProcessLabDataAsync(sampleTestId, request);
                return Ok(new { reponse });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An internal error occurred: " + ex.Message });
            }
        }
        [Authorize]
        [HttpPost("{sampleTestId}/detection-prep/result-Submit")]
        [CheckPermission("Save_Detection_Prep_Data")]
        public async Task<IActionResult> SaveDetectionPrepData([FromRoute] int sampleTestId, [FromBody] DetectionPrepResultDTO request)
        {
            if (request == null) return BadRequest("Request body cannot be null.");

            try
            {
                var reponse = await _labAnalysisService.ProcessDetectionAsync(sampleTestId, request);
                return Ok(new { reponse });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An internal error occurred: " + ex.Message });
            }
        }
    }
}