using TimeServer.Core.Models;

namespace TimeServer.Core.Strategies
{
    public interface ITimeStrategy
    {
        public Task<DateTime> GetCurrentDateTimeAsync();
        Task<IEnumerable<TimeRequestLog>> GetTimeLogsAsync(int page, int size);
    }
}
