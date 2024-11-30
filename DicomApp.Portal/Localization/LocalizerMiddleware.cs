﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DicomApp.Helpers;
using Microsoft.AspNetCore.Http;

namespace Localization
{
    public class LocalizerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Attempt to get the culture key from user claims
            var cultureKey = context
                .User.Claims.FirstOrDefault(c => c.Type == SystemConstants.Claims.Language)
                ?.Value;

            // If there is no culture key in claims, check the cookies
            if (string.IsNullOrEmpty(cultureKey))
            {
                // Try to retrieve the language from cookies
                cultureKey = context.Request.Cookies["PreferredLanguage"];
            }

            // If there is a culture key (either from claims or cookies)
            if (!string.IsNullOrEmpty(cultureKey) && DoesCultureExist(cultureKey))
            {
                // Set the culture Info
                var culture = new CultureInfo(cultureKey);

                // Set the culture in the current thread responsible for that request
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            // Continue with the next request
            await next(context);
        }

        private static bool DoesCultureExist(string cultureName)
        {
            // Return whether the culture exists
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Any(culture =>
                    string.Equals(
                        culture.Name,
                        cultureName,
                        StringComparison.CurrentCultureIgnoreCase
                    )
                );
        }
    }
}
