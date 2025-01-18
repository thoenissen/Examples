using BenchmarkDotNet.Attributes;

using BenchmarkUnitTests.Base;

namespace BenchmarkUnitTests.Benchmarks;

/// <summary>
/// Example benchmark
/// </summary>
public class ExampleBenchmark
{
    #region Benchmark

    /// <summary>
    /// <see cref="Thread.Sleep(int)"/>
    /// </summary>
    [Benchmark(Baseline = true)]
    [MaximumMean(20)]
    public void Sleep()
    {
        Thread.Sleep(100);
    }

    /// <summary>
    /// <see cref="Task.Delay(int)"/>
    /// </summary>
    /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
    [Benchmark]
    [MaximumMean(110)]
    public async Task Delay()
    {
        await Task.Delay(100);
    }

    #endregion // Benchmark
}