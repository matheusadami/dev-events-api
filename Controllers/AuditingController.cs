using System.Net.Mime;
using DevEventsApi.Entities;
using DevEventsApi.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevEventsApi.Controllers;

[ApiController]
[Tags("Auditoria")]
[Route("api/v1/auditings")]
[Produces(MediaTypeNames.Application.Json)]
public class AuditingController : ControllerBase
{
  public AuditingController(DatabaseContext context)
    => this.context = context;

  private DatabaseContext context { get; set; }

  /// <summary>
  /// Get all auditing records from database.
  /// </summary>
  /// <returns>Auditing records' list</returns>
  /// <response code="200">Success</response>
  [HttpGet]
  [Authorize(Roles = "CEO")]
  [ProducesResponseType(typeof(List<Auditing>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public IActionResult GetAll()
  {
    var records = context.Auditings.OrderByDescending(e => e.TimeStamp).ToList();
    return Ok(records);
  }
}