using Kirel.Logger.HTTP.DTOs;
using Kirel.DTO;
using Kirel.HttpLogger.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SortDirection = Kirel.Repositories.Sorts.SortDirection;

namespace Kirel.HttpLogger.API.Controllers;

/// <summary>
/// Controller for working with the http logs
/// </summary>
[Route("api/log/http/")]
[ApiController]
[Authorize]
public class KirelLogHttpController : Controller
{
    /// <summary>
    /// Http log service
    /// </summary>
    protected readonly KirelLogHttpService Service;
    
    /// <summary>
    /// Returns an instance of the http log db controller
    /// </summary>
    /// <param name="service">Http log service</param>
    public KirelLogHttpController(KirelLogHttpService service)
    {
        Service = service;
    }

    /// <summary>
    /// Gets log by id
    /// </summary>
    /// <param name="id">Log id</param>
    /// <returns>Log dto</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<KirelLogHttpDto>> GetById(Guid id)
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
    public virtual async Task<PaginatedResult<List<KirelLogHttpDto>>> GetList([FromQuery] int pageNumber = 0, int pageSize = 0,
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
    public virtual async Task<ActionResult<KirelLogHttpDto>> Create([FromBody] KirelLogHttpCreateDto createDto)
    {
        var dto = await Service.Create(createDto);
        if (dto != null) return Ok(dto);
        return BadRequest();
    }
}