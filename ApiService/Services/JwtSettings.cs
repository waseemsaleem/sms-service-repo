using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiService.Services
{
    public class JwtSettings
    {
        public JwtSettings(IConfiguration configuration)
        {
            ValidateIssuerSigningKey = Convert.ToBoolean(configuration.GetSection("JsonWebTokenKeys:ValidateIssuerSigningKey").Value);
            IssuerSigningKey = configuration.GetSection("JsonWebTokenKeys:IssuerSigningKey").Value;
            ValidateIssuer = Convert.ToBoolean(configuration.GetSection("JsonWebTokenKeys:ValidateIssuer").Value);
            ValidIssuer = configuration.GetSection("JsonWebTokenKeys:ValidIssuer").Value;
            ValidateAudience = Convert.ToBoolean(configuration.GetSection("JsonWebTokenKeys:ValidateAudience").Value);
            ValidAudience = configuration.GetSection("JsonWebTokenKeys:ValidAudience").Value;
            RequireExpirationTime = Convert.ToBoolean(configuration.GetSection("JsonWebTokenKeys:RequireExpirationTime").Value);
            ValidateLifetime = Convert.ToBoolean(configuration.GetSection("JsonWebTokenKeys:ValidateLifetime").Value);
        }
        public bool ValidateIssuerSigningKey { get; set; }
        public string? IssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; } = true;
        public string? ValidIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public string? ValidAudience { get; set; }
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; }
    }
}
