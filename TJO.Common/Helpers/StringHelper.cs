using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cultures = System.Globalization.CultureInfo;

namespace TJO.Common
{
    public static class StringHelper
    {
        public const string NullTextToken = "[null]";

        private static readonly System.Text.RegularExpressions.Regex rxHtmlLineBreak
            = new System.Text.RegularExpressions.Regex(
                @"<\s*br\s*/?\s*>|<\s*/?\s*p\s*>|<\s*\s*li\s*>",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

        private static readonly System.Text.RegularExpressions.Regex rxHtmlRemove
            = new System.Text.RegularExpressions.Regex(
                @"<[^>]*>",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

        private static readonly System.Text.RegularExpressions.Regex rxMultipleNewlines
            = new System.Text.RegularExpressions.Regex(
                @"(?:\r\n|\r(?!\n)|(?<!\r)\n){2,}",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

        private static readonly System.Text.RegularExpressions.Regex rxMultipleWhitespace
            = new System.Text.RegularExpressions.Regex(
                @"([ \t\v\f])[ \t\v\f]+",
                System.Text.RegularExpressions.RegexOptions.Compiled |
                System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                System.Text.RegularExpressions.RegexOptions.Multiline);

        private static readonly System.Text.RegularExpressions.Regex rxPascalCaseSplitter
            = new System.Text.RegularExpressions.Regex(
                @"(?<!^)([A-Z][a-z]|(?<=[a-z])[^a-z]|(?<=[A-Z])[0-9_])",
                System.Text.RegularExpressions.RegexOptions.Compiled);

        private static readonly char[] WhiteSpaceChars = new char[]
        {
            ' ', '\t', '\r', '\n', '\f', '\v',
        };

        public static string ToStringOrDefault(this string value, string defaultText = NullTextToken)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultText;
            else
                return value;
        }

        public static string ToStringOrDefault(this object value, string defaultText = NullTextToken)
        {
            if (!ReferenceEquals(null, value))
                return value.ToString();
            else
                return defaultText;
        }

        public static string ToStringOrDefault(this DateTime? value, string defaultText = NullTextToken)
        {
            if (value.HasValue)
                return value.Value.ToString();
            else
                return defaultText;
        }

        public static string ToDelimitedString<T>(this IEnumerable<T> values, string delimeter = ",", string defaultText = null)
        {
            if (ReferenceEquals(null, values) || (values.Count() == 0))
                return defaultText;
            else
                return string.Join(delimeter, values);
        }

        public static string ToStringOrNull(this object value)
        {
            if (!ReferenceEquals(null, value))
                return value.ToString();
            else
                return null;
        }

        public static string ToQuotedString(this object value, string quoteChar = "'", string defaultValue = NullTextToken)
        {
            return ToQuotedString(value, quoteChar, quoteChar, defaultValue);
        }

        public static string ToQuotedString(this object value, string startQuoteChar, string endQuoteChar, string defaultValue = NullTextToken)
        {
            var strValue = ToStringOrDefault(value, string.Empty);

            if (string.IsNullOrWhiteSpace(strValue))
                return defaultValue;
            else
                return startQuoteChar + value + endQuoteChar;
        }

        public static string ToUnquotedString(this string value, string quoteChar = "\"", string defaultValue = NullTextToken)
        {
            return ToUnquotedString(value, quoteChar, quoteChar, defaultValue);
        }

        public static string ToUnquotedString(this string value, string beginQuoteChar, string endQuoteChar, string defaultValue = NullTextToken)
        {
            var strValue = ToStringOrDefault(value, string.Empty);

            if (string.IsNullOrWhiteSpace(strValue))
                return defaultValue;
            else
            {
                return strValue
                    .TrimStart(WhiteSpaceChars.Concat(beginQuoteChar).ToArray())
                    .TrimEnd(WhiteSpaceChars.Concat(endQuoteChar).ToArray());
            }
        }

        public static string StripHtml(this string value, bool ignoreLineBreaks = false)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            var retVal = value;

            if (!ignoreLineBreaks)
            {
                retVal = rxHtmlLineBreak.Replace(retVal, Environment.NewLine);
            }

            retVal = rxHtmlRemove.Replace(retVal, "");

            retVal = rxMultipleNewlines.Replace(retVal, Environment.NewLine);

            retVal = rxMultipleWhitespace.Replace(retVal, "$1");
            return retVal.Trim();
        }

        public static string Truncate(this string value, int maxChars)
        {
            if ((value == null) || (maxChars <= 0))
                return value;

            else if (value.Length > maxChars)
                return value.Substring(0, maxChars);

            else
                return value;
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var ti = Cultures.CurrentCulture.TextInfo;
            string[] parts = SplitPascalCase(value)
                .Split()
                .Select(p => ti.ToTitleCase(p))
                .ToArray();

            if (parts.Length > 0)
            {
                parts[0] = parts[0].ToLower();
                return string.Join("", parts);
            }
            else if (value != value.ToUpper())
            {
                return value.Substring(0, 1).ToLower() + value.Substring(1);
            }

            return value;
        }

        public static string ToTitleCase(this string value)
        {
            return Cultures.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public static string ToTileCaseInvariant(this string value)
        {
            return Cultures.InvariantCulture.TextInfo.ToTitleCase(value);
        }

        private static string ToTitleCase(this System.Globalization.TextInfo ti, string value)
        {
            if (value == null) return null;
            var tokens = value.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = ti.ToUpper(token.Substring(0, 1)) + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }

        public static string SplitPascalCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return value;

            return rxPascalCaseSplitter.Replace(value, " $1");
        }

        public static string Coalesce(params string[] values)
        {
            return values.All(s => s != null)
                ? string.Join("", values)
                : null;
        }
    }
}
