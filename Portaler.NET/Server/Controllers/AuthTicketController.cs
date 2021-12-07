using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Portaler.NET.Shared;

namespace Portaler.NET.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthTicketController : ControllerBase
    {
        [HttpGet]
        public AuthTicket Get()
        {
            return new AuthTicket
            {
                Ticket = GetHashString(
                    Math.Pow(DateTime.UtcNow.DayOfYear / 2.0, 2).ToString(CultureInfo.CurrentCulture))
            };
        }
        
        private static IEnumerable<byte> GetHash(string inputString)
        {
            using HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}