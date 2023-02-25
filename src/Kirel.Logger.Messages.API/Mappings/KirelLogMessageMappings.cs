using AutoMapper;
using Kirel.Logger.Messages.API.Models;
using Kirel.Logger.Messages.DTOs;

namespace Kirel.Logger.Messages.API.Mappings;

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