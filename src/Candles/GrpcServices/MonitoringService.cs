using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Service.Candles.Contracts;
using Swisschain.Sdk.Server.Common;

namespace Candles.GrpcServices
{
    public class MonitoringService : Monitoring.MonitoringBase
    {
        public override Task<IsAliveResponse> IsAlive(Empty request, ServerCallContext context)
        {
            var result = new IsAliveResponse
            {
                Name = ApplicationInformation.AppName,
                Version = ApplicationInformation.AppVersion,
                StartedAt = ApplicationInformation.StartedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return Task.FromResult(result);
        }
    }
}
