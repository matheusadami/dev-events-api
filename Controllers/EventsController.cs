using System.Security.Claims;
using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DevEventsApi.Persistence;
using DevEventsApi.Entities;
using DevEventsApi.ViewModels.Event;
using DevEventsApi.ViewModels.EventSpeaker;
using DevEventsApi.Helpers;

namespace DevEventsApi.Controllers;

[ApiController]
[Tags("Eventos")]
[Route("api/v1/events")]
[Produces(MediaTypeNames.Application.Json)]
public class EventsController : ControllerBase
{
  private readonly DatabaseContext context;

  public EventsController(DatabaseContext context)
    => this.context = context;

  /// <summary>
  /// Get all events from database.
  /// </summary>
  /// <returns>Events' List</returns>
  /// <response code="200">Success</response>
  [HttpGet]
  [Authorize(Roles = "CEO")]
  [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetAll()
  {
    var events = await context.Events
      .Include(e => e.Speakers.OrderBy(s => s.Name))
      .Where(e => !e.DeletedAt.HasValue)
      .ToListAsync();

    return Ok(events);
  }

  /// <summary>
  /// Get event's data by uid.
  /// </summary>
  /// <param name="uid">Event's Uid</param>
  /// <returns>Event's Data</returns>
  /// <response code="200">Success</response>
  /// <response code="404">Event Not Found</response>
  [HttpGet("{uid}")]
  [Authorize]
  [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> GetByUid(Guid uid)
  {
    var response = await context.Events
      .Include(e => e.Speakers.OrderBy(s => s.Name))
      .SingleOrDefaultAsync(e => e.Uid.Equals(uid));

    if (response is null)
    {
      return NotFound();
    }

    return Ok(response);
  }

  /// <summary>
  /// Create a new event.
  /// </summary>
  /// <param name="model">Event's creating data</param>
  /// <returns>Newly Created Event</returns>
  /// <response code="201">Success</response>
  /// <response code="400">Event's Data Invalid</response>
  [HttpPost]
  [Authorize(Roles = "Manager,CEO")]
  [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Create([FromForm] CreateEventViewModel model)
  {
    var userIdentifier = AuthorizeHelper.GetUserIdentifierOrError(HttpContext);
    var register = new Event()
    {
      Title = model.Title,
      Description = model.Description,
      InitialDate = model.InitialDate,
      FinalDate = model.FinalDate,
      UserUid = new Guid(userIdentifier)
    };

    await context.Events.AddAsync(register);
    await context.SaveChangesAsync();

    return Created($"/{register.Uid}", register);
  }

  /// <summary>
  /// Update a event by uid.
  /// </summary>
  /// <param name="uid">Event's Uid</param>
  /// <param name="model">Event's updating data</param>
  /// <returns></returns>
  /// <response code="204">Success</response>
  /// <response code="404">Event Not Found</response>
  /// <response code="400">Event's Data Invalid</response>
  [HttpPut("{uid}")]
  [Authorize(Roles = "Manager,CEO")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Update(Guid uid, [FromForm] UpdateEventViewModel model)
  {
    var register = await context.Events.SingleOrDefaultAsync(e => e.Uid.Equals(uid));
    if (register is null)
    {
      return NotFound();
    }

    register.Title = model.Title ?? register.Title;
    register.Description = model.Description ?? register.Description;

    await context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Delete a event by uid.
  /// </summary>
  /// <param name="uid">Event's Uid</param>
  /// <returns></returns>
  /// <response code="204">Success</response>
  /// <response code="404">Not Found</response>
  [HttpDelete("{uid}")]
  [Authorize(Roles = "CEO")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> Delete(Guid uid)
  {
    var register = await context.Events.SingleOrDefaultAsync(e => e.Uid.Equals(uid));
    if (register is null)
    {
      return NotFound();
    }

    register.DeletedAt = DateTime.UtcNow;

    await context.SaveChangesAsync();

    return NoContent();
  }

  /// <summary>
  /// Create a new speaker.
  /// </summary>
  /// <param name="uid">Event's Uid</param>
  /// <param name="model">Speaker's creating data</param>
  /// <returns>Newly Created Speaker</returns>
  /// <response code="201">Success</response>
  /// <response code="404">Not Found</response>
  /// <response code="400">Event's data invalid</response>
  [HttpPost("{uid}/speakers")]
  [Authorize(Roles = "Manager,CEO")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<IActionResult> CreateSpeaker(Guid uid, [FromForm] CreateEventSpeakerViewModel model)
  {
    var register = await context.Events.SingleOrDefaultAsync(e => e.Uid.Equals(uid));
    if (register is null)
    {
      return NotFound();
    }

    var speaker = new EventSpeaker()
    {
      EventUid = register.Uid,
      Name = model.Name,
      Email = model.Email,
      TalkTitle = model.TalkTitle,
      TalkDescription = model.TalkDescription,
      LinkedInProfile = model.LinkedInProfile
    };

    await context.EventSpeakers.AddAsync(speaker);
    await context.SaveChangesAsync();

    var responseSpeaker = new
    {
      speaker.Uid,
      speaker.Name,
      speaker.Email,
      speaker.TalkTitle,
      speaker.TalkDescription,
      speaker.LinkedInProfile
    };

    return Created($"{register.Uid}/speakers/{speaker.Uid}", responseSpeaker);
  }
}