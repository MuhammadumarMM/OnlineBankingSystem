using Application.Common.Exceptions;
using Application.DTOs.CreditDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, SuperAdmin, User")]

public class CreditController(ICreditService creditService) : ControllerBase
{
    private readonly ICreditService _creditService = creditService;

    [HttpPost("add-Credit")]

    public async Task<IActionResult> AddAsync(AddCredit credit)
    {
        try
        {
            var result =    await _creditService.AddAsync(credit);
            return Ok(result);
        }
        catch(CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("get-all")]

    public async Task<IActionResult> GetAll()
    {
        try
        {
            var credits = await _creditService.GetAllAsync();
            return Ok(credits);
        }
        catch(CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpGet("get-by-id/{id}")]

    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var credit = await _creditService.GetByIdAsync(id);
            return Ok(credit);
        }
        catch(CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch(NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("get-by-userid/")]

    public async Task<IActionResult> GetByIdUser(string userId)
    {
        try
        {
            var credit = await _creditService.GetByUserIdAsync(userId);
            return Ok(credit);
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}
