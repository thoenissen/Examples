namespace BenchmarkUnitTests.Base;

/// <summary>
/// Time unit
/// </summary>
public enum TimeUnit : ulong
{
    /// <summary>
    /// Seconds
    /// </summary>
    Seconds = 1_000_000_000,

    /// <summary>
    /// Milliseconds
    /// </summary>
    Milliseconds = 1_000_000,

    /// <summary>
    /// Microseconds
    /// </summary>
    Microseconds = 1_000,

    /// <summary>
    /// Nanoseconds
    /// </summary>
    Nanoseconds = 1,
}