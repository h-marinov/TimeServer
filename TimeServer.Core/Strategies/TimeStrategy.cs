using TimeServer.Core.Models;
using TimeServer.Core.Ports;
using TimeServer.Core.Providers;

namespace TimeServer.Core.Strategies
{
    public class TimeStrategy : ITimeStrategy
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITimeLoggingPort _port;
        public TimeStrategy(IDateTimeProvider dateTimeProvider, ITimeLoggingPort port)
        {
            _dateTimeProvider = dateTimeProvider;
            _port = port;
        }

        public async Task<DateTime> GetCurrentDateTimeAsync()
        {
            var currentTime = _dateTimeProvider.GetCurrentDateTime();

            await _port.CreateLogAsync(new TimeRequestLog()
            {
                Time = currentTime,
            });

            return currentTime;
        }

        public async Task<IEnumerable<TimeRequestLog>> GetTimeLogsAsync(int page, int size)
        {
            int skip = (page - 1) * size;
            int take = size;

            return await _port.GetLogsAsync(skip, take);
        }
    }
}
