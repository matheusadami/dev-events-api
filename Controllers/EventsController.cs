using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DevEventsApi.Persistence;
using DevEventsApi.Entities;
using DevEventsApi.ViewModels.Event;
using DevEventsApi.ViewModels.EventSpeaker;

namespace DevEventsApi.Controllers;

[ApiController]
[Route("api/v1/events")]
[Produces("application/json")]
public class EventsController : ControllerBase
{
  private readonly DatabaseContext context;

  public EventsController(DatabaseContext context)
  {
    this.context = context;
  }

  /// <summary>
  /// Get all events from database.
  /// </summary>
  /// <returns>Events' List</returns>
  /// <response code="200">Success</response>
  [HttpGet]
  [ProducesResponseType(typeof(List<Event>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll()
  {
    var events = await context.Events
      .Include(e => e.Speakers.OrderBy(s => s.Name))
      .Where(e => !e.DeleteAt.HasValue)
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
  [ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
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
  [HttpPost]
  [ProducesResponseType(typeof(Event), StatusCodes.Status201Created)]
  public async Task<IActionResult> Create(CreateEventViewModel model)
  {
    var register = new Event()
    {
      Title = model.Title,
      Description = model.Description,
      InitialDate = model.InitialDate,
      FinalDate = model.FinalDate
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
  [HttpPut("{uid}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Update(Guid uid, UpdateEventViewModel model)
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
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Delete(Guid uid)
  {
    var register = await context.Events.SingleOrDefaultAsync(e => e.Uid.Equals(uid));
    if (register is null)
    {
      return NotFound();
    }

    register.DeleteAt = DateTime.UtcNow;

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
  [HttpPost("{uid}/speakers")]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> CreateSpeaker(Guid uid, CreateEventSpeakerViewModel model)
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
      speaker.TalkTitle,
      speaker.TalkDescription,
      speaker.LinkedInProfile
    };

    return Created($"{register.Uid}/speakers/{speaker.Uid}", responseSpeaker);
  }
}