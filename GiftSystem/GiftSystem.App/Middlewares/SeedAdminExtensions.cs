using Microsoft.AspNetCore.Builder;

namespace GiftSystem.App.Middlewares
{
    public static class SeedAdminExtensions
    {
        public static IApplicationBuilder SeedAdmin(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeedAdmin>();
        }
    }
}