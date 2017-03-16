using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TJO.TestTools.UnitTesting.Should.Core.Exceptions
{
    /// <summary>
    /// The base assert exception class
    /// </summary>

    public class AssertException : Exception
    {
        public static string FilterStackTraceAssemblyPrefix = "TJO.TestTools.UnitTesting.Should.";

        private readonly string _stackTrace;

        /// <summary>
        /// Initializes a new instance of <see cref="AssertException"/>.
        /// </summary>

        public AssertException() { }

        /// <summary>
        /// Initializes a new instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>

        public AssertException(string userMessage)
            : base(userMessage)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>
        /// <param name="innerException">The inner exception</param>

        protected AssertException(string userMessage, Exception innerException)
            : base(userMessage, innerException)
        {
            UserMessage = userMessage;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AssertException"/>.
        /// </summary>
        /// <param name="userMessage">The user message to be displayed</param>
        /// <param name="stackTrace">The stack trace to be displayed</param>

        protected AssertException(string userMessage, string stackTrace)
            : base(userMessage)
        {
            UserMessage = userMessage;
            _stackTrace = stackTrace;
        }

        /// <summary>
        /// Gets a string representation of the frames on the call stack at the 
        /// time the current exception was thrown.
        /// </summary>

        public override string StackTrace =>
            FilterStackTrace(_stackTrace ?? base.StackTrace);

        /// <summary>
        /// Filters the stack trace to remove all lines that occur within the 
        /// testing framework.
        /// </summary>
        /// <param name="stackTrace">The original stack trace</param>
        /// <returns>The filtered stack trace</returns>

        protected static string FilterStackTrace(string stackTrace)
        {
            if (stackTrace == null) return null;

            return string.Join(Environment.NewLine,
                SplitLines(stackTrace)
                    .Where(l => !l.TrimStart().StartsWith($"at {FilterStackTraceAssemblyPrefix}"))
                    .ToArray()
            );
        }

        private static IEnumerable<string> SplitLines(string input)
        {
            while (true)
            {
                int idx = input.IndexOf(Environment.NewLine);

                if (idx < 0)
                {
                    yield return input;
                    break;
                }

                yield return input.Substring(0, idx);
                input = input.Substring(idx + Environment.NewLine.Length);
            }
        }

        public string UserMessage { get; protected set; }
    }
}
