namespace BenchmarkUnitTests.Base;

/// <summary>
/// Maximum mean value of a report of the given method
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class MaximumMeanAttribute : Attribute
{
    #region Constructor

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="value">Value</param>
    /// <param name="unit">Unit</param>
    public MaximumMeanAttribute(double value, TimeUnit unit = TimeUnit.Milliseconds)
    {
        Value = value;
        Unit = unit;
    }

    #endregion // Constructor

    #region Properties

    /// <summary>
    /// Time unit
    /// </summary>
    public TimeUnit Unit { get; }

    /// <summary>
    /// Value
    /// </summary>
    public double Value { get; }

    #endregion // Properties

    #region Methods

    /// <summary>
    /// Getting <see cref="Value"/> in nanoseconds
    /// </summary>
    /// <returns>Value in nanoseconds</returns>
    internal double GetNanoseconds()
    {
        return Value * (ulong)Unit;
    }

    /// <summary>
    /// Getting the unit symbol
    /// </summary>
    /// <returns>Symbol</returns>
    internal string GetUnitSymbol()
    {
        return Unit switch
               {
                   TimeUnit.Seconds => "s",
                   TimeUnit.Milliseconds => "ms",
                   TimeUnit.Microseconds => "us",
                   TimeUnit.Nanoseconds => "ns",
                   _ => throw new ArgumentOutOfRangeException()
               };
    }

    /// <summary>
    /// Converting a value in nanoseconds into the unit defined by <see cref="Unit"/>
    /// </summary>
    /// <param name="value">Value</param>
    /// <returns>Converter Value</returns>
    internal double FromNanoseconds(double value)
    {
        return value / (ulong)Unit;
    }

    #endregion // Methods
}