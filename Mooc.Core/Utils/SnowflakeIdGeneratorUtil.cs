namespace Mooc.Core.Utils;

public static class SnowflakeIdGeneratorUtil
{
    //configuration parameter
    private const long Twepoch = 1609459200000L; // Twitter Epoch timestamp（2010-11-04T01:41:40.127Z）
    private const long WorkerIdBits = 3L;      // The number of digits occupied by the machine ID
    private const long DatacenterIdBits = 5L;  // The number of digits occupied by the data center ID
    private const long SequenceBits = 8L;     // The number of digits occupied by the serial number

    //Maximum value calculation
    private const long MaxWorkerId = -1L ^ (-1L << (int)WorkerIdBits);
    private const long MaxDatacenterId = -1L ^ (-1L << (int)DatacenterIdBits);
    private const long SequenceMask = -1L ^ (-1L << (int)SequenceBits);

    //shifting
    private const long WorkerIdShift = SequenceBits;
    private const long DatacenterIdShift = SequenceBits + WorkerIdBits;
    private const long TimestampLeftShift = SequenceBits + WorkerIdBits + DatacenterIdBits;

    //The timestamp of the last ID generation
    private static long lastTimestamp = -1L;

    //serial number
    private static long sequence = 0L;

    // Machine ID and Data Center ID
    private static readonly long workerId=5;
    private static readonly long datacenterId=5;

    //Lock object, used for thread safety
    private static readonly object lockObj = new object();

    //public SnowflakeIdGenerator(long workerId, long datacenterId)
    //{
    //    if (workerId > MaxWorkerId || workerId < 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(workerId), $"worker Id can't be greater than {MaxWorkerId} or less than 0");
    //    }

    //    if (datacenterId > MaxDatacenterId || datacenterId < 0)
    //    {
    //        throw new ArgumentOutOfRangeException(nameof(datacenterId), $"datacenter Id can't be greater than {MaxDatacenterId} or less than 0");
    //    }

    //    this.workerId = workerId;
    //    this.datacenterId = datacenterId;
    //}



    /// <summary>
    /// Generate the next ID
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static long NextId()
    {
        lock (lockObj)
        {
            var timestamp = TimeGen();

            if (timestamp < lastTimestamp)
            {
                throw new InvalidOperationException($"Clock moved backwards. Refusing to generate id for {lastTimestamp - timestamp} milliseconds");
            }

            if (lastTimestamp == timestamp)
            {
                sequence = (sequence + 1) & SequenceMask;
                if (sequence == 0)
                {
                    timestamp = TilNextMillis(lastTimestamp);
                }
            }
            else
            {
                sequence = 0L;
            }

            lastTimestamp = timestamp;

            return ((timestamp - Twepoch) << (int)TimestampLeftShift)
                   | (datacenterId << (int)DatacenterIdShift)
                   | (workerId << (int)WorkerIdShift)
                   | sequence;
        }
    }

    /// <summary>
    /// Get current timestamp (in milliseconds)
    /// </summary>
    /// <returns></returns>
    private static long TimeGen()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// Wait for the next millisecond until a new timestamp is obtained
    /// </summary>
    /// <param name="lastTimestamp"></param>
    /// <returns></returns>
    private static long TilNextMillis(long lastTimestamp)
    {
        var timestamp = TimeGen();
        while (timestamp <= lastTimestamp)
        {
            timestamp = TimeGen();
        }
        return timestamp;
    }

}
