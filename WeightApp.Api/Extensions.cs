using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WeightApp.Api
{
    public static class Extensions
    {
        public static IEnumerable<string> GetErrorMessages(this ModelStateDictionary dictionary)
        {
            return dictionary.SelectMany(m => m.Value.Errors)
                .Select(m => m.ErrorMessage)
                .ToList();
        }

        public static int GetUserId(this IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

            if (idClaim != null)
            {
                if (int.TryParse(idClaim.Value, out var id))
                    return id;
            }

            throw new AuthenticationException();
        }

        public static DateTime GetDateOfBirth(this IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(p => p.Type == ClaimTypes.DateOfBirth);

            if (idClaim != null)
            {
                if (DateTime.TryParse(idClaim.Value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dob))
                    return dob;
            }

            throw new AuthenticationException();
        }

        public static bool GetIsMale(this IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(p => p.Type == "isMale");

            if (idClaim != null)
            {
                if (bool.TryParse(idClaim.Value, out var id))
                    return id;
            }

            throw new AuthenticationException();
        }
    }
}