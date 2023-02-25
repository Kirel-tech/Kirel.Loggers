using AutoMapper;
using Kirel.DTO;
using Kirel.Logger.Messages.API.Context;
using Kirel.Logger.Messages.API.Models;
using Kirel.Logger.Messages.DTOs;
using Kirel.Repositories.Infrastructure.Generics;
using SortDirection = Kirel.Repositories.Sorts.SortDirection;

namespace Kirel.Logger.Messages.API.Services;

/// <summary>
/// Service that managing logs using generic repository 
/// </summary>
public class KirelLogMessageService
{
    private readonly KirelGenericEntityFrameworkRepository<Guid, KirelLogMessage, KirelLogMessageContext> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Returns instance of log service
    /// </summary>
    /// <param name="repository">Kirel generic repository instance</param>
    /// <param name="mapper">Class that represents <see cref="IMapper"/> interface</param>
    public KirelLogMessageService(KirelGenericEntityFrameworkRepository<Guid, KirelLogMessage, KirelLogMessageContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets log dto by log id
    /// </summary>
    /// <param name="id">Log unique identifier</param>
    /// <returns>Log dto</returns>
    public async Task<KirelLogMessageDto> GetById(Guid id)
    {
        var log = await _repository.GetById(id);
        return _mapper.Map<KirelLogMessageDto>(log);
    }
    
    /// <summary>
    /// Get paginated list of logs
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="orderBy">Sorting by field</param>
    /// <param name="orderDirection">Sorting direction</param>
    /// <param name="search">String to search in all fields</param>
    /// <returns>Paginated list of logs</returns>
    public async Task<PaginatedResult<List<KirelLogMessageDto>>> GetList(string search, string orderBy, SortDirection orderDirection, int page, int pageSize)
    {
        var log = await _repository.GetList(search, orderBy, orderDirection, page, pageSize);
        var pagination = Pagination.Generate(page ,pageSize, log.Count());
        var data = _mapper.Map<List<KirelLogMessageDto>>(log);
        return new PaginatedResult<List<KirelLogMessageDto>>
        {
            Pagination = pagination,
            Data = data
        };
    }

    /// <summary>
    /// Creates new log
    /// </summary>
    /// <param name="dto">Log create dto</param>
    /// <returns>Log dto</returns>
    public async Task<KirelLogMessageDto> Create(KirelLogMessageCreateDto dto)
    {
        var log = _mapper.Map<KirelLogMessage>(dto);
        var newLog = await _repository.Insert(log);
        return _mapper.Map<KirelLogMessageDto>(newLog);
    }
}