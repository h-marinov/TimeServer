using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using TimeServer.Api.Helpers;
using TimeServer.Core.Strategies;

namespace TimeServer.Api.Services
{
    [Authorize(Policy = Constants.AuthorizationPolicyCertificate)]
    public class TimeLogService : TimeLog.TimeLogBase
    {
        private readonly ILogger<TimeLogService> _logger;
        private readonly ITimeStrategy _timeStrategy;

        public TimeLogService(ILogger<TimeLogService> logger, ITimeStrategy timeStrategy)
        {
            _logger = logger;
            _timeStrategy = timeStrategy;
        }

        public override async Task<LogResponseDto> GetLogs(LogRequestDto request, ServerCallContext context)
        {
            var timeLogs = await _timeStrategy.GetTimeLogsAsync(request.Page, request.Size);

            _logger.LogTrace($"gRPC endpoint {nameof(GetLogs)} was executed successfully.");

            var response = new LogResponseDto()
            {
                Logs =
                {
                    timeLogs?.Select(tl => new LogResponseItemDto() { Time = DateTime.SpecifyKind(tl.Time, DateTimeKind.Utc).ToTimestamp() })
                    ?? new [] { new LogResponseItemDto() { } }
                }
            };

            return response;
        }
    }
}
