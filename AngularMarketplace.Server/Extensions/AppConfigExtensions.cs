namespace AngularMarketplace.Server.Extensions
{
    public static class AppConfigExtensions
    {

        public static WebApplication ConfigureCORS(this WebApplication app)
        {
            app.UseCors(
                x => x
                .WithOrigins("https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
            );
            return app;
        }
    }
}
