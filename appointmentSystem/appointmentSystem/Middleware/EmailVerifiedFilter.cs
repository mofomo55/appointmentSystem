using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace appointmentSystem.Middleware
{
    public class EmailVerifiedFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 1. Den aktuellen Benutzer aus dem Kontext extrahieren
            var user = context.HttpContext.User;

            var claimValue = user.FindFirstValue("email_verified");
            bool isEmailVerified = bool.TryParse(claimValue, out var result) && result;

            if (user.Identity.IsAuthenticated)
            {

                if (!isEmailVerified)
                {
                    // 3. Zugriff blockieren und eine 403- oder 400-Antwort zurückgeben
                    context.Result = new ObjectResult(new { message = "Please confirm the Email at First" })
                    {
                        StatusCode = 403
                    };
                    return;
                }
            }

            await next(); // Die Anfrage abschließen, falls die E-Mail-Adresse aktiv ist
        }
    }
}
