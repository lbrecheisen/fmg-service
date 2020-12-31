using System;

namespace Fmg.Models.Common
{
    public class Moment
    {
        public string? By { get; set; }
        public DateTimeOffset On
        {
            get => on;
            set
            {
                try { on = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(value, Timezone); }
                catch { on = value; }
            }
        }
        public long Timestamp { get => On.ToUniversalTime().ToUnixTimeSeconds(); }
        public string Timezone { get; set; } = TimeZoneInfo.Utc.Id;

        private DateTimeOffset on = DateTimeOffset.UtcNow;

        public Moment() { }
        public Moment(string? by, string timezone) => (By, Timezone) = (by, timezone);
    }
}