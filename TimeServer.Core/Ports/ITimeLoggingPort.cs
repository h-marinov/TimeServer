using TimeServer.Core.Models;

namespace TimeServer.Core.Ports
{
    public interface ITimeLoggingPort
    {
        Task<IEnumerable<TimeRequestLog>> GetLogsAsync(int skip, int take);
        Task<int> CreateLogAsync(TimeRequestLog log);
    }
}
