using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using TimeServer.Core.Strategies;

namespace TimeServer.Api.Services
{
    public class TimeService : Time.TimeBase
    {
        private readonly ILogger<TimeService> _logger;
        private readonly ITimeStrategy _timeStrategy;

        public TimeService(ILogger<TimeService> logger, ITimeStrategy timeStrategy)
        {
            _logger = logger;
            _timeStrategy = timeStrategy;
        }

        public override async Task<CurrentTimeResponseDto> GetCurrent(CurrentTimeRequestDto request, ServerCallContext context)
        {
            var time = await _timeStrategy.GetCurrentDateTimeAsync();

            _logger.LogTrace($"gRPC endpoint {nameof(GetCurrent)} was executed successfully.");

            return new CurrentTimeResponseDto
            {
                Time = time.ToTimestamp()
            };
        }
    }
}