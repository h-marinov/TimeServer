using TimeServer.Core.Models;

namespace TimeServer.DAL.Entities
{
    public class TimeRequestLogEntity
    {
        public TimeRequestLogEntity() { }

        public TimeRequestLogEntity(TimeRequestLog log)
        {
            Time = log.Time;
        }

        public int Id { get; set; }

        public DateTime Time { get; set; }
    }
}
