using System.Globalization;
using System.Reflection;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Extensions;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

using BenchmarkUnitTests.Benchmarks;

using Perfolizer.Mathematics.Histograms;

namespace BenchmarkUnitTests.Base;

/// <summary>
/// Test base class for creating unit tests based on benchmarks
/// </summary>
/// <typeparam name="TBenchmark">Type of the benchmark</typeparam>
/// <typeparam name="TConfiguration">Type of the configuration</typeparam>
public abstract class BenchmarkTestBase<TBenchmark, TConfiguration>
    where TConfiguration : IConfig, new()
{
    #region Fields

    /// <summary>
    /// Benchmark result
    /// </summary>
    private static Summary _results;

    #endregion // Fields

    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    static BenchmarkTestBase()
    {
        BenchmarkNames = typeof(TBenchmark).GetMethods()
                                           .Where(methodInfo => methodInfo.GetCustomAttribute<BenchmarkAttribute>() != null)
                                           .Select(methodInfo => new[] { methodInfo.Name })
                                           .ToArray();

    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Benchmark names
    /// </summary>
    public static IEnumerable<object[]> BenchmarkNames { get; }

    #endregion // Properties

    #region Tests

    /// <summary>
    /// Initialization
    /// </summary>
    /// <param name="context">Context</param>
    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static void Initialize(TestContext context)
    {
        _results = BenchmarkRunner.Run<ExampleBenchmark>(new TConfiguration());
    }

    /// <summary>
    /// Benchmarks
    /// </summary>
    /// <param name="name">Name</param>
    [TestMethod]
    [DynamicData(nameof(BenchmarkNames))]
    public void Benchmarks(string name)
    {
        var report = _results.Reports.FirstOrDefault(obj => obj.BenchmarkCase.Descriptor.WorkloadMethod.Name == name);

        Assert.IsNotNull(report, "Benchmark report could not be found.");
        Assert.IsNotNull(report.ResultStatistics, "No result statistics are available");

        WriteReport(report);

        var maximumMeanAttribute = report.BenchmarkCase.Descriptor.WorkloadMethod.GetCustomAttribute<MaximumMeanAttribute>();

        if (maximumMeanAttribute != null)
        {
            Assert.IsTrue(maximumMeanAttribute.GetNanoseconds() > report.ResultStatistics.Mean, $"The mean execution time limit ({maximumMeanAttribute.Value}{maximumMeanAttribute.GetUnitSymbol()}) is exceeded: {maximumMeanAttribute.FromNanoseconds(report.ResultStatistics.Mean)}{maximumMeanAttribute.GetUnitSymbol()}");
        }
    }

    #endregion // Tests

    #region Methods

    /// <summary>
    /// Write report to console
    /// </summary>
    /// <param name="report">Report</param>
    private void WriteReport(BenchmarkReport report)
    {
        var formatter = report.ResultStatistics!.CreateNanosecondFormatter(CultureInfo.InvariantCulture);

        Console.WriteLine(report.BenchmarkCase.DisplayInfo);
        Console.WriteLine($"Runtime = {report.GetRuntimeInfo() ?? "Unknown"}; GC = {report.GetGcInfo() ?? "Unknown"}");
        Console.WriteLine("-------------------- Histogram --------------------");
        Console.WriteLine(HistogramBuilder.Adaptive.Build(report.ResultStatistics!.OriginalValues).ToString(formatter));
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine(report.ResultStatistics!.ToString(CultureInfo.InvariantCulture, formatter, calcHistogram: false));
    }

    #endregion // Methods
}