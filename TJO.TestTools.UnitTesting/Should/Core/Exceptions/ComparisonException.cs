using System;
using System.Collections.Generic;
using System.Text;

namespace TJO.TestTools.UnitTesting.Should.Core.Exceptions
{
    public abstract class ComparisonException : AssertException
    {
        public string Left { get; }
        public string Right { get; }

        protected ComparisonException(object left, object right, string methodName, string operation)
            : base($"Assert.{methodName}() Failure:\r\n\tExpected: {Format(right)} {operation} {Format(left)}\r\n\tbut it was not")
        {
            Left = left?.ToString();
            Right = right?.ToString();
        }

        protected ComparisonException(object left, object right, string message) : base(message)
        {
            Left = left?.ToString();
            Right = right?.ToString();
        }

        private static object Format(object value)
        {
            if (value == null)
            {
                return "(null)";
            }

            var type = value.GetType();
            return type == typeof(string)
                ? $"\"{value}\""
                : value.ToString();
        }
    }
}
