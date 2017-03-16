using System;
using System.Collections.Generic;
using System.Text;
using RES = TJO.Common.Properties.Resources;

namespace TJO.Common
{
    public static class Argument_Extensions
    {
        public static void ThrowIfNull<T>(this T argValue, string argName)
            where T: class
        {
            if (ReferenceEquals(null, argValue))
            {
                throw new ArgumentNullException(RES.ExArgumentNull,
                    string.IsNullOrEmpty(argName) ? RES.TokenUnnamedArgument : argName);
            }
        }

        public static void ThrowIfNullOrWhiteSpace(this string argValue, string argName)
        {
            ThrowIfNullOrWhiteSpace(argValue, argName, RES.ExArgumentNull);
        }

        public static void ThrowIfNullOrWhiteSpace(this string argValue, string argName, string message)
        {
            if (string.IsNullOrWhiteSpace(argValue))
            {
                throw new ArgumentNullException(message,
                    string.IsNullOrEmpty(argName) ? RES.TokenUnnamedArgument : argName);
            }
        }

        public static void ThrowIfNotAssignableTo<T>(this Type argType, string argName)
        {
            ThrowIfNotAssignableTo<T>(argType, argName,
                string.Format(RES.ExNotAssignableToTypeFormat, argType?.FullName.ToStringOrDefault(RES.TokenNull)));
        }

        public static void ThrowIfNotAssignableTo<T>(this Type argType, string argName, string message)
        {

        }
    }
}
