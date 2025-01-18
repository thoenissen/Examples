using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;

namespace BenchmarkUnitTests.Base;

/// <summary>
/// Default configuration
/// </summary>
public class DefaultConfiguration : ManualConfig
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    public DefaultConfiguration()
    {
        AddJob(Job.Default);
        AddLogger(NullLogger.Instance);
    }

    #endregion // Constructor
}