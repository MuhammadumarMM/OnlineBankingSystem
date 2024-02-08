using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.DTOs.BankCardDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = IdentityRoles.SUPER_ADMIN)]

public class BankCardController(IBankCardService bankCardService) : ControllerBase
{
    private readonly IBankCardService _bankCardService = bankCardService;

    [HttpPost("add-bankcard")]
    public async Task<IActionResult> AddBankCard(AddBankCardDto bankCard)
    {
        try
        {
            await _bankCardService.AddAsync(bankCard);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (NotFoundException ex)
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
            var credits = await _bankCardService.GetAllAsync();
            return Ok(credits);
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("delete/{id}")]

    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _bankCardService.DeleteAsync(id);
            return Ok();
        }
        catch (CustomException ex)
        {
            return BadRequest(ex.Message);

        }
        catch (NotFoundException ex)
        {
            return NotFound();
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
            var credit = await _bankCardService.GetByIdAsync(id);
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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync(UpdateBankCardDto bankCardDto)
    {
        try
        {
            await _bankCardService.UpdateAsync(bankCardDto);
            return Ok(bankCardDto);
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
