using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using WebApiJwt.DTO;

namespace WebApiJwt.Controllers;

/// <summary>
/// Authentication API
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    #region Fields

    /// <summary>
    /// Configuration
    /// </summary>
    private readonly IConfiguration _configuration;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration"></param>
    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    #endregion // Constructor

    #region Routes

    /// <summary>
    /// Creation of the jwt
    /// </summary>
    /// <param name="loginRequest">Login request data</param>
    /// <returns>Created JWT</returns>
    [HttpPost]
    [Route("createToken")]
    public IActionResult CreateToken([FromBody] LoginRequestDTO loginRequest)
    {
        // TODO Validation of login request

        return Ok(CreateToken(loginRequest.UserName));
    }


    /// <summary>
    /// Creation of the jwt
    /// </summary>
    /// <returns>Created JWT</returns>
    [HttpPost]
    [Route("singleSingOn")]
    [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
    public IActionResult CreateTokenWithSingleSignOn()
    {
        if (HttpContext.User.Identity is not { IsAuthenticated: true })
        {
            return Unauthorized();
        }

        // TODO Additional authentication steps

        return Ok(CreateToken(HttpContext.User.Identity.Name));

    }

    #endregion // Routes

    #region Methods

    /// <summary>
    /// Token creation
    /// </summary>
    /// <param name="name">User name</param>
    /// <returns>JWT</returns>
    private string CreateToken(string name)
    {
        // TODO Additional steps to determinate the roles of a user

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                                 _configuration["Jwt:Issuer"],
                                                 [
                                                     new Claim(ClaimTypes.Name, name),
                                                     new Claim(ClaimTypes.Role, "ExampleRole")
                                                 ],
                                                 expires: DateTime.Now.AddMinutes(120),
                                                 signingCredentials: credentials);
        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    #endregion // Methods
}