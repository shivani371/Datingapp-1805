using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext context, ITokenService tokenService): BaseApiController
{
    [HttpPost("register")] //api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto model)
    {
        if(await EmailExists(model.Email)) return BadRequest("Bad Request Email already exists");
        using var hmac= new HMACSHA512();
        var user= new AppUser
        {
            DisplayName=model.DisplayName,
            Emain=model.Email,
            PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(model.PassWord)),
            PasswordSalt=hmac.Key
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user.ToDto(tokenService);


    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto login)
    {
        var user=await context.Users.SingleOrDefaultAsync(s=>s.Emain==login.Email );
        if(user== null) return Unauthorized("Inavlid email adress");

       using var hmac= new HMACSHA512(user.PasswordSalt);

       var computedHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(login.PassWord));

       for(int i=0; i<computedHash.Length; i++)
        {
            if(computedHash[i]!= user.PasswordHash[i]) return Unauthorized("Inavlid password");
        }
        return user.ToDto(tokenService);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await context.Users.AnyAsync(x=>x.Emain.ToLower()== email.ToLower());
    }

}
