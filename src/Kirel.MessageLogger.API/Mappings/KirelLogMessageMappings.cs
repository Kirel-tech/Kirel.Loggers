using AutoMapper;
using Kirel.MessageLogger.API.Models;
using Kirel.MessageLogger.DTOs;

namespace Kirel.MessageLogger.API.Mappings;

/// <summary>
/// Mapper of <see cref="KirelLogMessage"/> entities
/// </summary>
public class KirelLogMessageMappings : Profile
{
    /// <summary>
    /// Returns instance of log mapping 
    /// </summary>
    public KirelLogMessageMappings()
    {
        CreateMap<KirelLogMessage, KirelLogMessageDto>().ReverseMap();
        CreateMap<KirelLogMessage, KirelLogMessageCreateDto>().ReverseMap();
    }
}