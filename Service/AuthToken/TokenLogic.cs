using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Context;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces.TokenInterfaces;

namespace Service.AuthToken
{
    public class TokenLogic:IToken
    {
        public readonly AppDbContext _context;
        public readonly IConfiguration _configuration;
        public TokenLogic(AppDbContext context, IConfiguration configuration, HttpClient httpClient)
        {
            _context = context;
            _configuration = configuration;
        }
        public string CreateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtKey = Environment.GetEnvironmentVariable("Key");

            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new Exception("JWT secret key is not set in the environment variables.");
            }
            var key = Encoding.UTF8.GetBytes(jwtKey);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Role, user.Role)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<RefreshToken> CreateRefreshTokenAsync(User user)
        {
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = Guid.NewGuid().ToString(),
                ExpirationDate = DateTime.Now.AddDays(7),
                UserId = user.Id,
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }
        public async Task<string> RefreshAccessTokenAsync(string refreshTokenInput)
        {
            var refreshToken = await _context.RefreshTokens.Include(el => el.User)
                .Include(el => el.User)
                .FirstOrDefaultAsync(el => el.Token == refreshTokenInput);
            if (refreshToken == null || refreshToken.ExpirationDate < DateTime.Now)
            {
                throw new SecurityTokenException("Refresh token expired or invalid");
            }
            var newAccessToken = CreateAccessToken(refreshToken.User);
            var newRefreshToken = await CreateRefreshTokenAsync(refreshToken.User);

            _context.RefreshTokens.Remove(refreshToken);
            await _context.SaveChangesAsync();
            return newAccessToken;
        }
    }
}
