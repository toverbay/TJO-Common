namespace TJO.TestTools.UnitTesting.Should.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a collection should not contain the expected value, but does.
    /// </summary>

    public class DoesNotContainException : AssertException
    {
        /// <summary>
        /// Creates a new instance of <see cref="DoesNotContainException"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>

        public DoesNotContainException(object expected)
            : base($"Assert.DoesNotContain() failure: Found: {expected}")
        {
        }
    }
}
