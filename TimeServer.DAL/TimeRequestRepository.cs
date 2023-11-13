using Microsoft.EntityFrameworkCore;
using TimeServer.Core.Models;
using TimeServer.Core.Ports;
using TimeServer.DAL.Entities;

namespace TimeServer.DAL
{
    public class TimeRequestRepository : ITimeLoggingPort
    {
        public async Task<int> CreateLogAsync(TimeRequestLog log)
        {
            using var db = new TimeServerContext();

            db.Add(new TimeRequestLogEntity(log));

            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TimeRequestLog>> GetLogsAsync(int skip, int take)
        {
            using var db = new TimeServerContext();

            var logs = await db.TimeRequestLogs.Skip(skip).Take(take).ToListAsync();

            return logs.Select(l => new TimeRequestLog()
            {
                Time = l.Time
            });
        }
    }
}
