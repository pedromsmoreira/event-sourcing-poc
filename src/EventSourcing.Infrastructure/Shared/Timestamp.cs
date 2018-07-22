namespace EventSourcing.Infrastructure.Shared
{
    using System;

    public class Timestamp
    {
        public static readonly DateTime UnixTimeEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

       public Timestamp()
        {
            this.UnixTimeEpochTimestamp = (int)DateTime.UtcNow.Subtract(UnixTimeEpoch).TotalSeconds;
        }

        public int UnixTimeEpochTimestamp { get; }
    }
}