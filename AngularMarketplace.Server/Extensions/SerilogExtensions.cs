using Serilog;
using System.Runtime.CompilerServices;

namespace AngularMarketplace.Server.Extensions
{
    public static class SerilogExtensions
    {
        public static ConfigureHostBuilder ConfigureSerilog(this ConfigureHostBuilder host,IConfiguration configuration)
        {
            host.UseSerilog((ctx, lc) =>
             {

                 lc.WriteTo.Logger(l =>
                    l.Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Information).
                        WriteTo.
                        File(
                            configuration["AppSettings:LogPath"],
                            rollingInterval: RollingInterval.Day
                            )).
                   WriteTo.Logger(l =>
                   l.Filter.ByIncludingOnly(x => x.Level == Serilog.Events.LogEventLevel.Error).
                        WriteTo.
                        File(
                        configuration["AppSettings:LogPathError"],
                        rollingInterval: RollingInterval.Day
                        ));
             });

            return host;
        }
    }
}
