using AutoMapper;
using Kirel.HttpLogger.DTOs;
using Kirel.DTO;
using Kirel.HttpLogger.API.Context;
using Kirel.HttpLogger.API.Models;
using Kirel.Repositories.Infrastructure.Generics;
using SortDirection = Kirel.Repositories.Sorts.SortDirection;

namespace Kirel.HttpLogger.API.Services;

/// <summary>
/// Service that managing http logs using generic repository 
/// </summary>
public class KirelLogHttpService
{
    private readonly KirelGenericEntityFrameworkRepository<Guid, KirelLogHttp, KirelLogHttpDbContext> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Returns instance of http log service
    /// </summary>
    /// <param name="repository">Kirel generic repository instance</param>
    /// <param name="mapper">Class that represents <see cref="IMapper"/> interface</param>
    public KirelLogHttpService(KirelGenericEntityFrameworkRepository<Guid, KirelLogHttp, KirelLogHttpDbContext> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets http log dto by log id
    /// </summary>
    /// <param name="id">Log unique identifier</param>
    /// <returns>Http log dto</returns>
    public async Task<KirelLogHttpDto> GetById(Guid id)
    {
        var log = await _repository.GetById(id);
        return _mapper.Map<KirelLogHttpDto>(log);
    }
    
    /// <summary>
    /// Get paginated list of http logs
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="orderBy">Sorting by field</param>
    /// <param name="orderDirection">Sorting direction</param>
    /// <param name="search">String to search in all fields</param>
    /// <returns>Paginated list of http logs</returns>
    public async Task<PaginatedResult<List<KirelLogHttpDto>>> GetList(string search, string orderBy, SortDirection orderDirection, int page, int pageSize)
    {
        var log = await _repository.GetList(search, orderBy, orderDirection, page, pageSize);
        var pagination = Pagination.Generate(page ,pageSize, log.Count());
        var data = _mapper.Map<List<KirelLogHttpDto>>(log);
        return new PaginatedResult<List<KirelLogHttpDto>>
        {
            Pagination = pagination,
            Data = data
        };
    }

    /// <summary>
    /// Creates new http log
    /// </summary>
    /// <param name="dto">Http log create dto</param>
    /// <returns>Http log dto</returns>
    public async Task<KirelLogHttpDto> Create(KirelLogHttpCreateDto dto)
    {
        var log = _mapper.Map<KirelLogHttp>(dto);
        var newLog = await _repository.Insert(log);
        return _mapper.Map<KirelLogHttpDto>(newLog);
    }
}