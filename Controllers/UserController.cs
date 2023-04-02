using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using DevEventsApi.Entities;
using DevEventsApi.Persistence;
using DevEventsApi.Services.Interfaces;
using DevEventsApi.ViewModels.User;

namespace DevEventsApi.Controllers;

[ApiController]
[Tags("Autenticação e Usuários")]
[Route("api/v1/users")]
[Produces(MediaTypeNames.Application.Json)]
public class UserController : Controller
{
  public UserController(ITokenService tokenService, DatabaseContext databaseContext)
  => (this.tokenService, this.context) = (tokenService, databaseContext);

  private DatabaseContext context { get; set; }

  private ITokenService tokenService { get; set; }

  /// <summary>
  /// Generate a new auth token for being used to the API endpoints.
  /// </summary>
  /// <param name="model">User's data</param>
  /// <returns>User's data and new Token</returns>
  /// <returns code="200">Success</returns>
  /// <returns code="400">Bad request</returns>
  [HttpPost("generate-token")]
  [ProducesResponseType(typeof(ResponseGenerateTokenViewModel), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> GenerateToken(GenerateTokenViewModel model)
  {
    try
    {
      var user = await context.Users
        .SingleOrDefaultAsync(e => e.Username.Equals(model.Username) && e.Password.Equals(model.Password));

      if (user is null)
        throw new Exception("Username ou senha inválidos.");

      var token = tokenService.GenerateToken(user);

      return Ok(new ResponseGenerateTokenViewModel
      {
        User = user,
        Token = token
      });
    }
    catch (Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }

  /// <summary>
  /// Get all users from database.
  /// </summary>
  /// <returns>Users' List</returns>
  /// <response code="200">Success</response>
  [HttpGet]
  [Authorize(Roles = "CEO")]
  [ProducesResponseType(typeof(List<User?>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll()
  {
    var users = await context.Users
        .Include(e => e.Events.OrderBy(e => e.CreatedAt))
        .ToListAsync();

    return Ok(users);
  }

  /// <summary>
  /// Create a new user.
  /// </summary>
  /// <param name="model">User's data</param>
  /// <returns code="201">Success</returns>
  /// <returns code="400">Bad Request</returns>
  [HttpPost]
  [ProducesResponseType(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  public async Task<IActionResult> CreateUser([FromForm] CreateUserViewModel model)
  {
    try
    {
      var IsAlreadyExistsUsername = context.Users.Any(e => e.Username.Equals(model.Username));
      if (IsAlreadyExistsUsername)
        throw new Exception("Falha no cadastro de usuário. Tente outro username.");

      var user = new User
      {
        Name = model.Name,
        Role = model.Role,
        Username = model.Username,
        Password = model.Password
      };
      await context.Users.AddAsync(user);
      await context.SaveChangesAsync();

      return Created($"/{user.Uid}", new { user.Uid, user.Name, user.Username, user.Role });
    }
    catch (System.Exception ex)
    {
      return BadRequest(new { error = ex.Message });
    }
  }
}
