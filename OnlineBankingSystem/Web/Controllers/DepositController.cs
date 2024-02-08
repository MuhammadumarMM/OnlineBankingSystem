using Application.Common.Exceptions;
using Application.DTOs.CreditDtos;
using Application.DTOs.DepositDtos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, SuperAdmin, User")]

public class DepositController (IDepositService depositService) : ControllerBase
{
    private readonly IDepositService _depositService = depositService;


    [HttpPost("add-deposit")]
    public async Task<IActionResult> Post(AddDeposit addDeposit)
    {
        try
        {
            await _depositService.AddAsync(addDeposit);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
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


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await _depositService.GetAllAsync();
            return Ok(result);
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

    [HttpGet("get-by-id/{id}")]

    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var resalt = await _depositService.GetByIdAsync(id);
            return Ok(resalt);
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

    [HttpGet("get-by-userid/")]

    public async Task<IActionResult> GetByIdUser(string userId)
    {
        try
        {
            var credit = await _depositService.GetByUserIdAsync(userId);
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
