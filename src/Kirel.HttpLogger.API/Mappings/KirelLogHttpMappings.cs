using AutoMapper;
using Kirel.Logger.HTTP.DTOs;
using Kirel.HttpLogger.API.Models;

namespace Kirel.HttpLogger.API.Mappings;

/// <summary>
/// Mapper of <see cref="KirelLogHttp"/> entities
/// </summary>
public class KirelLogHttpMappings : Profile
{
    /// <summary>
    /// Returns instance of http log mapping 
    /// </summary>
    public KirelLogHttpMappings()
    {
        CreateMap<KirelLogHttp, KirelLogHttpDto>().ReverseMap();
        CreateMap<KirelLogHttp, KirelLogHttpCreateDto>().ReverseMap();
    }
}