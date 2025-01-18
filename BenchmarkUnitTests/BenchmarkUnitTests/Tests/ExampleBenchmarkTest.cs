using BenchmarkUnitTests.Base;
using BenchmarkUnitTests.Benchmarks;

namespace BenchmarkUnitTests.Tests;

/// <summary>
/// Execution of <see cref="ExampleBenchmark"/>
/// </summary>
[TestClass]
public class ExampleBenchmarkTest : BenchmarkTestBase<ExampleBenchmark, DefaultConfiguration>;