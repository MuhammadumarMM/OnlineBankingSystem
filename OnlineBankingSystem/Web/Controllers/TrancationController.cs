using Application.Common.Exceptions;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, SuperAdmin, User")]

    public class TrancationController(ITrancationService trancationService) : ControllerBase
    {
        private readonly ITrancationService _trancationService = trancationService;

        [HttpPost("trancation")]
        public async Task<IActionResult> Trancation([FromBody] TrancationRequestDto trancationRequest)
        {
            try
            {
                await _trancationService.InTransaction(trancationRequest.GiveCard, trancationRequest.TakeCard, trancationRequest.Amount);
                return Ok();
            }
            catch(CustomException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
