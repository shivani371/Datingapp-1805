using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{      
    private readonly IConfiguration configuration;
    public TokenService(IConfiguration configuration)
    {
        this.configuration=configuration;
    } 

   public string CreateToken(AppUser user)
    {
       var tokenKey=configuration["TokenKey"]?? throw new Exception("Token key not found");
       if(tokenKey.Length<64) throw new Exception("your token key need to be >64");

       var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

       var claims= new List<Claim>
       {
           new (ClaimTypes.Email, user.Emain),
           new (ClaimTypes.NameIdentifier, user.Id)
       };

       var creds=new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

       var tokenDescriptor= new SecurityTokenDescriptor
       {
           Subject=new ClaimsIdentity(claims),
           Expires= DateTime.UtcNow.AddDays(7),
           SigningCredentials=creds
       };

       var tokenHandler= new JwtSecurityTokenHandler();
       var token= tokenHandler.CreateToken(tokenDescriptor);

       return tokenHandler.WriteToken(token);   

    }
}
