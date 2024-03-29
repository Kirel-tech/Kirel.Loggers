﻿using AutoMapper;
using Kirel.Blazor.Entities.Models;
using Kirel.Logger.Messages.DTOs;
using Microsoft.AspNetCore.Components;

namespace Kirel.Logger.Messages.Blazor.Pages;

/// <inheritdoc />
public partial class LogMessageEntityDialog
{
    /// <summary>
    /// Microsoft http client factory
    /// </summary>
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    [Inject]
    protected IMapper Mapper { get; set; } = null!;
    /// <summary>
    /// Data transfer object for get the entity
    /// </summary>
    [Parameter]
    public KirelLogMessageDto? Dto { get; set; }
    /// <summary>
    /// Options for control dialog and fields entity settings
    /// </summary>
    [Parameter]
    public EntityOptions? Options { get; set; }

    /// <summary>
    /// Additional dialog content
    /// </summary>
    [Parameter] public RenderFragment? AdditionalContent { get; set; }
    /// <summary>
    /// Dialog properties section additional content
    /// </summary>
    [Parameter] public RenderFragment? PropertiesAdditionalContent { get; set; }

    /// <summary>
    /// Http client instance name
    /// </summary>
    [Parameter]
    public string HttpClientName { get; set; } = "MessageLogs";
    /// <summary>
    /// Relative url to API endpoint
    /// </summary>
    [Parameter]
    public string? HttpRelativeUrl { get; set; }
    /// <summary>
    /// Before create request event handler
    /// </summary>
    [Parameter]
    public Func<KirelLogMessageDto?, Task>? BeforeCreateRequest { get; set; }
    /// <summary>
    /// Before update request event handler
    /// </summary>
    [Parameter]
    public Func<KirelLogMessageDto?, Task>? BeforeUpdateRequest { get; set; }
    
    private HttpClient _httpClient = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _httpClient = HttpClientFactory.CreateClient(HttpClientName);
        var action = EntityAction.Read;
        Options ??= new EntityOptions()
        {
            Action = action,
        };
        Options.IgnoredProperties.Add("StackTrace");
        await base.OnInitializedAsync();
    }
}