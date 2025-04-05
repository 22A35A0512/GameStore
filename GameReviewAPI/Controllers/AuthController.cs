using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using System.Text;
using System.Threading.Tasks;
using System;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    [HttpGet("reviews/{gameId}")]
    public async Task<IActionResult> GetReviews(int gameId)
    {
        var reviews = await _context.Reviews
            .Where(r => r.GameId == gameId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return Ok(reviews);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (string.IsNullOrEmpty(registerRequest.Username))
                return BadRequest(new { message = "Username is required" });
            if (string.IsNullOrEmpty(registerRequest.Email))
                return BadRequest(new { message = "Email is required" });
            if (string.IsNullOrEmpty(registerRequest.FullName))
                return BadRequest(new { message = "Full name is required" });
            if (string.IsNullOrEmpty(registerRequest.Password))
                return BadRequest(new { message = "Password is required" });

        if (await _context.Users.AnyAsync(u => u.Username == registerRequest.Username))
            return BadRequest(new { message = "Username already exists" });

        var user = new User
        {
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            FullName = registerRequest.FullName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Registration successful" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);

        if (user == null)
        {
            Console.WriteLine($"User {loginRequest.Username} not found");
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
        Console.WriteLine($"Password validation result for {loginRequest.Username}: {passwordValid}");

        if (!passwordValid)
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var token = GenerateJwtToken(user.Username);
        return Ok(new { message = "Successfully logged in", token, username = user.Username });
    }
    // 
    [HttpPost("submit-review")]
    public async Task<IActionResult> SubmitReview([FromBody] ReviewRequest reviewRequest)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == reviewRequest.GameId);
        if (game == null)
        {
            return NotFound(new { message = "Game not found" });
        }

        var newReview = new Review
        {
            Username = reviewRequest.Username,
            GameId = reviewRequest.GameId,
            ReviewText = reviewRequest.ReviewText,
            Rating = reviewRequest.Rating,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reviews.Add(newReview);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Review submitted successfully" });
    }


    private string GenerateJwtToken(string username)
    {
        var key = new SymmetricSecurityKey(Convert.FromBase64String(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
