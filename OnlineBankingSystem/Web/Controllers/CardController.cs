using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.DTOs.CardDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, SuperAdmin, User")]
public class CardController(ICardService cardService) : ControllerBase
{
    private readonly ICardService _cardService = cardService;

    [HttpPost("add-card")]

    public async Task<IActionResult> AddAsync(AddCard card)
    {
        try
        {
            await _cardService.AddAsync(card);
            return Ok();
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
            var cards = await _cardService.GetAllAsync();
            return Ok(cards);
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

    [HttpDelete("delete/{id}")]

    public async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            await _cardService.DeleteAsync(id);
            return Ok();
        }
        catch(CustomException ex)
        {
            return BadRequest(ex.Message);

        }
        catch(NotFoundException ex)
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
            var card = await _cardService.GetByIdAsync(id);
            return Ok(card);
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


    [HttpGet("get-by-UserId")]

    public async Task<IActionResult> GetByUserId(string userId)
    {
        try
        {
            var card = await _cardService.GetByUserId(userId);
            return Ok(card);
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
