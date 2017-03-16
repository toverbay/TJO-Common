namespace TJO.TestTools.UnitTesting.Should.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a collection should contain the expected value, but does not.
    /// </summary>

    public class ContainsException : AssertException
    {
        public ContainsException(object expected)
            : base($"Assert.Contains() failure: Not found: {expected}")
        {
        }
    }
}
