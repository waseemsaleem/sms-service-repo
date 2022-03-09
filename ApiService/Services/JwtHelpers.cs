using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public static class JwtHelpers
    {
        public static string GenTokenkey(JwtSettings jwtSettings, List<Claim> claims)
        {
            // Get secret key
            var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
            Guid Id = Guid.Empty;
            DateTime expireTime = DateTime.UtcNow.AddHours(1);
            var token = new JwtSecurityToken(issuer: jwtSettings.ValidIssuer, audience: jwtSettings.ValidAudience, claims: claims, notBefore: new DateTimeOffset(DateTime.UtcNow).DateTime, expires: new DateTimeOffset(expireTime).DateTime, signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
