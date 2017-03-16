using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TJO.TestTools.UnitTesting.Should.Core.Assertions;

namespace TJO.TestTools.UnitTesting.Should.Core.Exceptions
{
    /// <summary>
    /// Base class for exceptions that have actual and expected values
    /// </summary>

    public class AssertActualExpectedException : AssertException
    {
        private readonly string _actual;
        private readonly string _differencePosition = "";
        private readonly string _expected;

        /// <summary>
        /// Creates a new instance of <see cref="AssertActualExpectedException"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="userMessage">The user message to be shown</param>

        public AssertActualExpectedException(object expected,
                                             object actual,
                                             string userMessage)
            : this(expected, actual, userMessage, false) { }

        /// <summary>
        /// Creates a new instance of <see cref="AssertActualExpectedException"/>.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="userMessage">The user message to be shown</param>
        /// <param name="skipPositionCheck">True to skip the check for difference position; Otherwise, false</param>

        public AssertActualExpectedException(object expected,
                                             object actual,
                                             string userMessage,
                                             bool skipPositionCheck)
            : base(userMessage)
        {
            if (!skipPositionCheck)
            {
                var enumerableActual = actual as IEnumerable;
                var enumerableExpected = expected as IEnumerable;

                if (enumerableActual != null && enumerableExpected != null)
                {
                    var comparer = new EnumerableEqualityComparer();
                    comparer.Equals(enumerableActual, enumerableExpected);

                    _differencePosition = $"Position: First difference is at position {comparer.Position}{Environment.NewLine}";
                }
            }

            _actual = actual == null ? null : ConverToString(actual);
            _expected = expected == null ? null : ConverToString(expected);

            if (actual != null &&
                expected != null &&
                actual.ToString() == expected.ToString() &&
                actual.GetType() != expected.GetType())
            {
                _actual += $" ({actual.GetType().FullName})";
                _expected += $" ({expected.GetType().FullName})";
            }

            _lazyMessage = new Lazy<string>(() =>
            {
                return $"{base.Message}{Environment.NewLine}{_differencePosition}Expected: {FormatMultiLine(Expected ?? "(null)")}{Environment.NewLine}Actual:    {FormatMultiLine(Actual ?? "(null)")}";
            });
        }

        /// <summary>
        /// Gets the string representation of the actual value
        /// </summary>

        public string Actual => _actual;

        /// <summary>
        /// Gets the string representation of the expected value
        /// </summary>

        public string Expected => _expected;

        private readonly Lazy<string> _lazyMessage;

        /// <summary>
        /// Gets a message that describes the current exception. Includes the expected and actual values.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string ("").</returns>
        /// <filterpriority>1</filterpriority>

        public override string Message => _lazyMessage.Value;
            

        private static object FormatMultiLine(string value)
        {
            return value.Replace(Environment.NewLine, $"{Environment.NewLine}          ");
        }

        private static string ConverToString(object value)
        {
            var valueArray = value as Array;
            if (valueArray == null)
            {
                return value.ToString();
            }

            var valueStrings = new List<string>();

            foreach (var valueObject in valueArray)
            {
                valueStrings.Add(valueObject == null ? "(null)" : valueObject.ToString());
            }

            return $"{value.GetType().FullName} {{ {string.Join(", ", valueStrings.ToArray())} }}";
        }
    }
}
