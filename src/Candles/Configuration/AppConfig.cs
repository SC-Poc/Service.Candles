using Candles.Configuration.Jwt;
using Candles.Configuration.Service;

namespace Candles.Configuration
{
    public class AppConfig
    {
        public CandlesServiceSettings CandlesService { get; set; }

        public JwtConfig Jwt { get; set; }
    }
}
