using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WarehouseAPI.DTOs;
using WarehouseAPI.Helpers;

namespace WarehouseAPI.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
        IConfiguration configuration, ApplicationDbContext dbContext, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _dbContext = dbContext;
        _mapper = mapper;
    }

    [HttpGet("listUsers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<ActionResult<List<UserDTO>>> GetListUsers([FromQuery] PaginationDTO paginationDto)
    {
        var queryable = _dbContext.Users.AsQueryable();
        await HttpContext.InsertParametersPaginationHeader(queryable);
        var users = await queryable.Paginate(paginationDto).ToListAsync();
        return _mapper.Map<List<UserDTO>>(users);
    }

    [HttpPost("makeAdmin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public async Task<ActionResult> MakeAdmin([FromBody] string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.AddClaimAsync(user, new Claim("role", "admin"));
        return NoContent();
    }

    [HttpPost("create")]
    public async Task<ActionResult<AuthenticationResponse>> Create([FromBody] UserCredentials userCredentials)
    {
        var user = new IdentityUser { UserName = userCredentials.Email, Email = userCredentials.Email };
        var result = await _userManager.CreateAsync(user, userCredentials.Password);
        if (result.Succeeded)
        {
            return BuildToken(userCredentials);
        }

        else
        {
            return BadRequest(result.Errors);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] UserCredentials userCredentials)
    {
        var result = await _signInManager.PasswordSignInAsync(userCredentials.Email, userCredentials.Password,
            isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            return BuildToken(userCredentials);
        }

        else
        {
            return BadRequest("Incorrect login");
        }
    }

    private AuthenticationResponse BuildToken(UserCredentials userCredentials)
    {
        var claims = new List<Claim>()
        {
            new Claim("email", userCredentials.Email)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddYears(1);
        var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration,
            signingCredentials: creds);
        return new AuthenticationResponse()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}