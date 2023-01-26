using Kirel.DTO;
using Kirel.MessageLogger.API.Services;
using Kirel.MessageLogger.DTOs;
using Microsoft.AspNetCore.Mvc;
using SortDirection = Kirel.Repositories.Sorts.SortDirection;

namespace Kirel.MessageLogger.API.Controllers;

/// <summary>
/// Controller for working with the logs
/// </summary>
[Route("api/log/")]
public class KirelLogMessageController : Controller
{
    /// <summary>
    /// Log service
    /// </summary>
    protected readonly KirelLogMessageService Service;
    
    /// <summary>
    /// Returns an instance of the log db controller
    /// </summary>
    /// <param name="service">Log service</param>
    public KirelLogMessageController(KirelLogMessageService service)
    {
        Service = service;
    }

    /// <summary>
    /// Gets log by id
    /// </summary>
    /// <param name="id">Log id</param>
    /// <returns>Log dto</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<KirelLogMessageDto>> GetById(Guid id)
    {
        var result = await Service.GetById(id);
        if (result != null) return Ok(result);
        return NotFound();
    }
    
    /// <summary>
    /// Gets logs paginated list
    /// </summary>
    /// <param name="pageNumber">The number of the displayed page</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="orderBy">Name of the sorting field</param>
    /// <param name="orderDirection">Order direction</param>
    /// <param name="search">Search string parameter</param>
    /// <returns>Paginated result with list of logs dto</returns>
    [HttpGet]
    public virtual async Task<PaginatedResult<List<KirelLogMessageDto>>> GetList([FromQuery] int pageNumber = 0, int pageSize = 0,
        string orderBy = "", string orderDirection = "asc", string search = "")
    {
        var directionEnum = SortDirection.Asc;
        if (orderDirection == "desc")
        {
            directionEnum = SortDirection.Desc;
        }
        return await Service.GetList(search, orderBy, directionEnum, pageNumber, pageSize);
    }
    
    /// <summary>
    /// Create new log
    /// </summary>
    /// <param name="createDto">Log create dto</param>
    /// <returns>Log dto</returns>
    [HttpPost]
    public virtual async Task<ActionResult<KirelLogMessageDto>> Create([FromBody] KirelLogMessageCreateDto createDto)
    {
        var dto = await Service.Create(createDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }
}